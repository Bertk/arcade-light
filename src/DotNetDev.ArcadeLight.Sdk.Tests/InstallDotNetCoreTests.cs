namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  using System;

  using FluentAssertions;

  using Xunit;

  using DotNetDev.ArcadeLight.Test.Common;
  using System.IO;
  using Microsoft.Build.Framework;
  using Moq;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Linq;
  using Xunit.Sdk;
  using DotNetDev.ArcadeLight.Sdk;

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
      InstallDotNetCore customTask = new InstallDotNetCore();
      customTask.GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json"));
      customTask.DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd"));
      customTask.BuildEngine = buildEngine.Object;
      //Act and Assert
      var success = customTask.Execute();
      Assert.True(success);
    }

    [Fact]
    public void InstallDotNetCoreNoGlobalJsonFile()
    {
      //Arrange
      InstallDotNetCore customTask = new InstallDotNetCore();
      customTask.GlobalJsonPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "global.json"));
      customTask.DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd"));
      customTask.BuildEngine = buildEngine.Object;
      //Act and Assert
      var success = customTask.Execute();
      Assert.True(success);
      // warning list is not available
    }

    [Fact]
    public void InstallDotNetCoreNoInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new InstallDotNetCore();
      customTask.GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json"));
      customTask.DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"xxx.cmd"));
      customTask.BuildEngine = buildEngine.Object;
      //Act and Assert
      var success = customTask.Execute();
      Assert.False(success);
      Assert.NotEmpty(errors);
    }

    [Theory]
    [InlineData("x86", "x86")]
    [InlineData("x64", "x64")]
    [InlineData("", "x64")]
    public void GetArchitectureVerify(string architecture, string value)
    {
      //Arrange
      InstallDotNetCore customTask = new InstallDotNetCore();

      //Act
      var result = customTask.GetArchitecture(architecture);

      //Assert
      Assert.Equal(value, result);

    }
  }
}
