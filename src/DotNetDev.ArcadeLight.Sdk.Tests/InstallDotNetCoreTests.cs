using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class InstallDotNetCoreTests
  {
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;
    private readonly string projectRootDir;
    private readonly string repositoryEngineeringDir;

    public InstallDotNetCoreTests()
    {
      buildEngine = new Mock<IBuildEngine4>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

      projectRootDir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../../../../"));
      repositoryEngineeringDir = Path.GetFullPath(Path.Combine(projectRootDir, "eng"));
    }


    [Fact]
    public void InstallDotNetCoreVerify()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.True(success);
      Assert.Empty(errors);
    }

    [Fact]
    public void InstallDotNetCoreNoGlobalJsonFile()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      // returns success and cretes a warning
      Assert.True(success);
      // warning list is not available and errors are empty
      Assert.Empty(errors);

    }

    [Fact]
    public void InstallDotNetCoreNoInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"xxx.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.False(success);
      Assert.NotEmpty(errors);
      Assert.Contains("Unable to find dotnet install script", errors[0].Message);
    }
    [Fact]
    public void TryInstallDotNetRuntimeWithInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.Combine(projectRootDir, @"src/DotNetDev.ArcadeLight.Sdk.Tests/testassets/installVersion/global.json"),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.False(success);
      Assert.NotEmpty(errors);
      Assert.Contains("dotnet-install failed", errors[0].Message);
    }
  }
}
