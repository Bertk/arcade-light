// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Sdk
{
  public class LocateDotNet : Microsoft.Build.Utilities.Task
  {
    private const string s_cacheKey = "LocateDotNet-FCDFF825-F35B-4601-9CB5-74DCA498B589";

    private sealed class CacheEntry
    {
      public readonly DateTime LastWrite;
      public readonly string Paths;
      public readonly string Value;

      public CacheEntry(DateTime lastWrite, string paths, string value)
      {
        LastWrite = lastWrite;
        Paths = paths;
        Value = value;
      }
    }

    [Required]
    public string RepositoryRoot { get; set; }

    [Output]
    public string DotNetPath { get; set; }

    private static readonly char[] separator = new[] { ';' };

    public override bool Execute()
    {
      ExecuteImpl();
      return !Log.HasLoggedErrors;
    }

    private void ExecuteImpl()
    {
      string globalJsonPath = Path.Combine(RepositoryRoot, "global.json");

      DateTime lastWrite = File.GetLastWriteTimeUtc(globalJsonPath);
      string paths = Environment.GetEnvironmentVariable("PATH");

      CacheEntry cachedResult = (CacheEntry)BuildEngine4.GetRegisteredTaskObject(s_cacheKey, RegisteredTaskObjectLifetime.Build);
      if (cachedResult != null && lastWrite == cachedResult.LastWrite && paths == cachedResult.Paths)
      {
        Log.LogMessage(MessageImportance.Low, $"Reused cached value.");
        DotNetPath = cachedResult.Value;
        return;
      }

      string globalJson = File.ReadAllText(globalJsonPath);

      // avoid Newtonsoft.Json dependency
      Match match = Regex.Match(globalJson, @"""dotnet""\s*:\s*""([^""]+)""");
      if (!match.Success)
      {
        Log.LogError($"Unable to determine dotnet version from file '{globalJsonPath}'.");
        return;
      }

      string sdkVersion = match.Groups[1].Value;

      string fileName = (Path.DirectorySeparatorChar == '\\') ? "dotnet.exe" : "dotnet";
#pragma warning disable S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
      string dotNetDir = paths.Split(separator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(p => File.Exists(Path.Combine(p, fileName)));
#pragma warning restore S6602 // "Find" method should be used instead of the "FirstOrDefault" extension

      if (dotNetDir == null || !Directory.Exists(Path.Combine(dotNetDir, "sdk", sdkVersion)))
      {
        Log.LogError($"Unable to find dotnet with SDK version '{sdkVersion}'");
        return;
      }

      DotNetPath = Path.GetFullPath(Path.Combine(dotNetDir, fileName));
      BuildEngine4.RegisterTaskObject(s_cacheKey, new CacheEntry(lastWrite, paths, DotNetPath), RegisteredTaskObjectLifetime.Build, allowEarlyCollection: true);
    }
  }
}
