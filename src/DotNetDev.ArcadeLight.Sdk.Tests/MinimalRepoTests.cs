// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using DotNetDev.ArcadeLight.Sdk.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  [Collection(TestProjectName.Name)]
  public class MinimalRepoTests
  {
    private readonly ITestOutputHelper _output;
    private readonly TestProjectFixture _fixture;

    public MinimalRepoTests(ITestOutputHelper output, TestProjectFixture fixture)
    {
      _output = output;
      _fixture = fixture;
    }

    [Fact(Skip = "https://github.com/dotnet/arcade/issues/7092")]
    public void MinimalRepoBuildsWithoutErrors()
    {
      using TestApp app = _fixture.CreateTestApp("MinimalRepo");
      int exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
      Assert.Equal(0, exitCode);
    }

    [Fact(Skip = "https://github.com/dotnet/arcade/issues/7092")]
    public void MinimalRepoWithFinalVersions()
    {
      using TestApp app = _fixture.CreateTestApp("MinimalRepo");
      int exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false",
                "/p:DotNetFinalVersionKind=release");
      Assert.Equal(0, exitCode);
    }
  }
}
