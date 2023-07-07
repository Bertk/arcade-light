// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests.Utilities
{
  [CollectionDefinition(Name)]
  public class TestProjectName : ICollectionFixture<TestProjectFixture>
  {
    public const string Name = nameof(TestProjectName);
  }

  public class TestProjectFixture : IDisposable
  {
    private readonly ConcurrentQueue<IDisposable> _disposables = new ConcurrentQueue<IDisposable>();
    private readonly string _logOutputDir;
    private readonly string _testAssets;
    private readonly string _boilerPlateDir;

    private static readonly string[] _packagesToClear =
    {
            "DotNet.ArcadeLight.Sdk",
        };

    public TestProjectFixture()
    {
      ClearPackages();
      _logOutputDir = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "LogOutputDir").Value;
      _testAssets = Path.Combine(AppContext.BaseDirectory, "testassets");
      _boilerPlateDir = Path.Combine(_testAssets, "boilerplate");
    }

    public TestApp CreateTestApp(string name)
    {
      string testAppFiles = Path.Combine(_testAssets, name);
      string instanceName = Path.GetRandomFileName();
      string tempDir = Path.Combine(Path.GetTempPath(), "arcade", instanceName);
      TestApp app = new TestApp(tempDir, _logOutputDir, new[] { testAppFiles, _boilerPlateDir });
      _disposables.Enqueue(app);
      return app;
    }

    private void ClearPackages()
    {
      string nugetRoot = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "NuGetPackageRoot").Value;
      string pkgVersion = GetType().Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "PackageVersion").Value;
      foreach (string package in _packagesToClear)
      {
        string pkgRoot = Path.Combine(nugetRoot, package, pkgVersion);
        if (Directory.Exists(pkgRoot))
        {
          Directory.Delete(pkgRoot, recursive: true);
        }
      }
    }

    // Dispose() calls Dispose(true)
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Cleanup
      while (!_disposables.IsEmpty)
      {
        if (_disposables.TryDequeue(out IDisposable disposable))
        {
          disposable.Dispose();
        }
      }
    }

  }
}
