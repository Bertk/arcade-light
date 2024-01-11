// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Xunit.Abstractions;

namespace DotNetDev.ArcadeLight.Sdk.Tests.Utilities
{

  public class TestApp : IDisposable
  {
    private readonly string _logOutputDir;

    public TestApp(string workDir, string logOutputDir, string[] sourceDirectories)
    {
      WorkingDirectory = workDir;
      _logOutputDir = Path.Combine(logOutputDir, Path.GetFileName(workDir));

      ArgumentNullException.ThrowIfNull(sourceDirectories);

      Directory.CreateDirectory(workDir);
      Directory.CreateDirectory(_logOutputDir);

      foreach (string dir in sourceDirectories)
      {
        CopyRecursive(dir, workDir);
      }
    }

    public string WorkingDirectory { get; }

    private static readonly string[] first = new[] { "-bl" };

    public int ExecuteBuild(ITestOutputHelper output, params string[] scriptArgs)
    {
      string cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
              ? @".\build.cmd"
              : "./build.sh";

      ArgumentNullException.ThrowIfNull(output);

      return ExecuteScript(output, cmd, first.Concat(scriptArgs));
    }

    private int ExecuteScript(ITestOutputHelper output, string fileName, IEnumerable<string> scriptArgs)
    {
      output.WriteLine("Working dir = " + WorkingDirectory);
      output.WriteLine("Log output  = " + _logOutputDir);

      string cmd = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
              ? "cmd.exe"
              : "bash";

      ProcessStartInfo psi = new()
      {
        FileName = cmd,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        UseShellExecute = false,

        WorkingDirectory = WorkingDirectory,
      };

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        psi.ArgumentList.Add("/C");
      }

      psi.ArgumentList.Add(fileName);
      psi.ArgumentList.AddRange(scriptArgs);

      return Run(output, psi);
    }

    public int Run(ITestOutputHelper output, ProcessStartInfo psi)
    {
      void Write(object sender, DataReceivedEventArgs e)
      {
        output.WriteLine(e.Data ?? string.Empty);
      }

      ArgumentNullException.ThrowIfNull(psi);
      psi.UseShellExecute = false;
      psi.RedirectStandardError = true;
      psi.RedirectStandardOutput = true;
#pragma warning disable S125 // Sections of code should not be commented out
      //psi.Environment["PATH"] = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH");
#pragma warning restore S125 // Sections of code should not be commented out

      Process process = new()
      {
        StartInfo = psi,
        EnableRaisingEvents = true
      };
      process.OutputDataReceived += Write;
      process.ErrorDataReceived += Write;
      output?.WriteLine($"Starting: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
      process.Start();
      process.BeginErrorReadLine();
      process.BeginOutputReadLine();

      process.WaitForExit(1000 * 60 * 3);

      CopyRecursive(Path.Combine(WorkingDirectory, "artifacts", "log"), _logOutputDir);

      process.OutputDataReceived -= Write;
      process.ErrorDataReceived -= Write;
      return process.ExitCode;
    }

    private static void CopyRecursive(string srcDir, string destDir)
    {
      foreach (string srcFileName in Directory.EnumerateFiles(srcDir, "*", SearchOption.AllDirectories))
      {
        string destFileName = Path.Combine(destDir, srcFileName.Substring(srcDir.Length).TrimStart(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
        File.Copy(srcFileName, destFileName);
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
      // Cleanup
      try
      {
        Directory.Delete(WorkingDirectory, recursive: true);
      }
      catch (Exception e) when (e is ArgumentException or UnauthorizedAccessException or IOException)
      {
        // Sometimes anti virus scanning locks files and they can't be deleted. Retrying after 500ms seems to get around this most of the time
        Thread.Sleep(500);
        Directory.Delete(WorkingDirectory, recursive: true);
      }
    }
  }
}
