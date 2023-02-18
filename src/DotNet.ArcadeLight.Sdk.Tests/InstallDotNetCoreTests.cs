namespace DotNet.ArcadeLight.Sdk.Tests
{
  using System;

  using FluentAssertions;

  using Xunit;

  using DotNet.ArcadeLight.Test.Common;
  using System.IO;
  using Microsoft.Build.Framework;
  using Moq;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Linq;

  public class InstallDotNetCoreTests
  {
    private MockRepository mockRepository;
    private Mock<IBuildEngine> buildEngine;
    private List<BuildErrorEventArgs> errors;
    private readonly string projectRootDir;
    private readonly string repositoryEngineeringDir;

    public InstallDotNetCoreTests()
    {
      this.mockRepository = new MockRepository(MockBehavior.Strict);
      buildEngine = new Mock<IBuildEngine>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

      projectRootDir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\..\..\"));
      repositoryEngineeringDir = Path.GetFullPath(Path.Combine(projectRootDir, "eng"));
    }


    [Fact]
    public void ExecuteHappyTest ()
    {
      //Arrange
      InstallDotNetCore customTask = new InstallDotNetCore ();
      customTask.GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json"));
      customTask.DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight\dotnet-install.cmd"));
      //customTask.GlobalJsonPath = @"..\..\..\global.json";
      customTask.BuildEngine = buildEngine.Object;
      //Act and Assert
      var success = customTask.Execute ();
      Assert.True(success);
      Assert.Empty(errors);
      this.mockRepository.VerifyAll();
    }

    //[Fact]
    //public void GetArchitectureTest ()
    //{
    //  //Arrange
    //  var architecture = "x86";
    //  Type type = typeof(InstallDotNetCore);
    //  var installDotNetCore = Activator.CreateInstance(type, architecture);
    //  MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == "GetArchitecture" && x.IsPrivate)
    //  .First();

    //  //Act
    //  var result = (string)method.Invoke(installDotNetCore, new object[] { architecture });

    //  //Assert
    //  Assert.Equal("x64", result);

    // }

    [Theory]
    [InlineData("x86", "x86")]
    [InlineData("x64", "x64")]
    [InlineData("", "x64")]
    public void GetArchitectureTest(string architecture, string value)
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
