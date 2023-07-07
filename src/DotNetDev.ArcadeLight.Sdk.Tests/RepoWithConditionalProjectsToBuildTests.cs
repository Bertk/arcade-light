// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Runtime.InteropServices;
using DotNetDev.ArcadeLight.Sdk.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  [Collection(TestProjectName.Name)]
  public class RepoWithConditionalProjectsToBuildTests
  {
    private readonly ITestOutputHelper _output;
    private readonly TestProjectFixture _fixture;

    public RepoWithConditionalProjectsToBuildTests(ITestOutputHelper output, TestProjectFixture fixture)
    {
      _output = output;
      _fixture = fixture;
    }

    [Theory(Skip = "https://github.com/dotnet/arcade/issues/7092")]
    [InlineData(false, 1, false)]
    [InlineData(false, 1, true)]
    [InlineData(true, 2, false)]
    [InlineData(true, 2, true)]
    public void RepoProducesPackages(bool buildAdditionalProject, int expectedPackages, bool stablePackages)
    {
      TestApp app = _fixture.CreateTestApp("RepoWithConditionalProjectsToBuild");
      string packArg = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "-pack"
                : "--pack";
      string finalVersionKindArgument = stablePackages ? "/p:DotNetFinalVersionKind=release" : "/p:DotNetFinalVersionKind=prerelease";
      int exitCode = app.ExecuteBuild(_output,
                packArg,
                $"/p:ShouldBuildMaybe={buildAdditionalProject}",
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false",
                finalVersionKindArgument);
      Assert.Equal(0, exitCode);
      string[] nupkgFiles = Directory.GetFiles(Path.Combine(app.WorkingDirectory, "artifacts", "packages", "Debug", "Shipping"), "*.nupkg");

      _output.WriteLine("Packages produced:");

      foreach (string file in nupkgFiles)
      {
        _output.WriteLine(file);
      }

      Assert.Equal(expectedPackages, nupkgFiles.Length);
    }
  }
}
