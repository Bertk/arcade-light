// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetDev.ArcadeLight.Common
{
  public class Command : ICommand, IDisposable
  {
    public static readonly string[] RunnableSuffixes = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? new string[] { ".exe", ".cmd", ".bat" }
        : new string[] { string.Empty };

    private readonly Process _process;

    private Action<string> _statusForward;

    private StringWriter _stdOutCapture;
    private StringWriter _stdErrCapture;

    private Action<string> _stdOutForward;
    private Action<string> _stdErrForward;

    private Action<string> _stdOutHandler;
    private Action<string> _stdErrHandler;

    private bool _running;
    private bool _quietBuildReporter;

    internal Command(string executable, string args)
    {
      // Set the things we need
      ProcessStartInfo psi = new()
      {
        FileName = executable,
        Arguments = args
      };

      _process = new Process()
      {
        StartInfo = psi
      };
    }

    public ICommand QuietBuildReporter()
    {
      _quietBuildReporter = true;
      return this;
    }

    public CommandResult Execute()
    {
      ThrowIfRunning();
      _running = true;

      if (_process.StartInfo.RedirectStandardOutput)
      {
        _process.OutputDataReceived += (sender, args) =>
        {
          ProcessData(args.Data, _stdOutCapture, _stdOutForward, _stdOutHandler);
        };
      }

      if (_process.StartInfo.RedirectStandardError)
      {
        _process.ErrorDataReceived += (sender, args) =>
        {
          ProcessData(args.Data, _stdErrCapture, _stdErrForward, _stdErrHandler);
        };
      }

      _process.EnableRaisingEvents = true;

      if (_process.StartInfo.RedirectStandardOutput ||
          _process.StartInfo.RedirectStandardInput ||
          _process.StartInfo.RedirectStandardError)
      {
        _process.StartInfo.UseShellExecute = false;
      }

      Stopwatch sw = Stopwatch.StartNew();
      ReportExecBegin();

      _process.Start();

      if (_process.StartInfo.RedirectStandardOutput)
      {
        _process.BeginOutputReadLine();
      }

      if (_process.StartInfo.RedirectStandardError)
      {
        _process.BeginErrorReadLine();
      }

      _process.WaitForExit();

      int exitCode = _process.ExitCode;

      ReportExecEnd(exitCode);

      return new CommandResult(
          _process.StartInfo,
          exitCode,
          _stdOutCapture?.GetStringBuilder()?.ToString(),
          _stdErrCapture?.GetStringBuilder()?.ToString());
    }

    public ICommand WorkingDirectory(string projectDirectory)
    {
      _process.StartInfo.WorkingDirectory = projectDirectory;
      return this;
    }

    public ICommand EnvironmentVariable(string name, string value)
    {
#if NET45
            _process.StartInfo.EnvironmentVariables[name] = value;
#else
      _process.StartInfo.Environment[name] = value;
#endif
      _process.StartInfo.UseShellExecute = false;
      return this;
    }

    public ICommand ForwardStatus(TextWriter toStatus = null)
    {
      ThrowIfRunning();
      if (toStatus == null)
      {
        _statusForward = Console.WriteLine;
      }
      else
      {
        _statusForward = toStatus.WriteLine;
      }
      return this;
    }

    public ICommand CaptureStdOut()
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardOutput = true;
      _stdOutCapture = new StringWriter();
      return this;
    }

    public ICommand CaptureStdErr()
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardError = true;
      _stdErrCapture = new StringWriter();
      return this;
    }

    public ICommand ForwardStdOut(TextWriter toOut = null)
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardOutput = true;
      if (toOut == null)
      {
        _stdOutForward = Console.WriteLine;
      }
      else
      {
        _stdOutForward = toOut.WriteLine;
      }
      return this;
    }

    public ICommand ForwardStdErr(TextWriter toErr = null)
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardError = true;
      if (toErr == null)
      {
        _stdErrForward = Console.WriteLine;
      }
      else
      {
        _stdErrForward = toErr.WriteLine;
      }
      return this;
    }

    public ICommand OnOutputLine(Action<string> handler)
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardOutput = true;
      if (_stdOutHandler != null)
      {
        throw new InvalidOperationException("Already handling stdout!");
      }
      _stdOutHandler = handler;
      return this;
    }

    public ICommand OnErrorLine(Action<string> handler)
    {
      ThrowIfRunning();
      _process.StartInfo.RedirectStandardError = true;
      if (_stdErrHandler != null)
      {
        throw new InvalidOperationException("Already handling stderr!");
      }
      _stdErrHandler = handler;
      return this;
    }

    private static string FormatProcessInfo(ProcessStartInfo info, bool includeWorkingDirectory)
    {
      string prefix = includeWorkingDirectory ?
          $"{info.WorkingDirectory}> {info.FileName}" :
          info.FileName;

      if (string.IsNullOrWhiteSpace(info.Arguments))
      {
        return prefix;
      }

      return prefix + " " + info.Arguments;
    }

    private void ReportExecBegin()
    {
      if (!_quietBuildReporter && _statusForward != null)
      {
        _statusForward($"[EXEC Begin] {Command.FormatProcessInfo(_process.StartInfo, includeWorkingDirectory: false)}");
      }
    }

    private void ReportExecEnd(int exitCode)
    {
      if (!_quietBuildReporter && _statusForward != null)
      {
        bool success = exitCode == 0;

        string message = $"{Command.FormatProcessInfo(_process.StartInfo, includeWorkingDirectory: !success)} exited with {exitCode}";

        _statusForward($"[EXEC End] {message}");
      }
    }

    private void ThrowIfRunning([CallerMemberName] string memberName = null)
    {
      if (_running)
      {
        throw new InvalidOperationException($"Unable to invoke {memberName} after the command has been run");
      }
    }

    private static void ProcessData(string data, StringWriter capture, Action<string> forward, Action<string> handler)
    {
      if (data == null)
      {
        return;
      }

      capture?.WriteLine(data);

      forward?.Invoke(data);

      handler?.Invoke(data);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Dispose managed resources.
        _process.Dispose();
        _stdOutCapture.Dispose();
        _stdErrCapture.Dispose();

      }
      // Free native resources.
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
