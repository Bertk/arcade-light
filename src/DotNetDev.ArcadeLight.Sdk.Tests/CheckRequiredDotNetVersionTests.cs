using DotNetDev.ArcadeLight.Sdk;
using Microsoft.Build.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class CheckRequiredDotNetVersionTests
  {
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;
    private readonly string repositoryRoot;


    public CheckRequiredDotNetVersionTests()
    {
      buildEngine = new Mock<IBuildEngine4>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

      repositoryRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../../../../"));

    }

    [Theory]
    [InlineData("7.0.302", true)]
    [InlineData("7.0.888", true)]
    public void CheckRequiredDotNetVersionVerify(string minSdkVersionStr, bool expectedResult)
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new CheckRequiredDotNetVersion();
      checkRequiredDotNetVersion.BuildEngine = buildEngine.Object;
      checkRequiredDotNetVersion.RepositoryRoot = repositoryRoot;
      checkRequiredDotNetVersion.SdkVersion = minSdkVersionStr;
      // Act
      var result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.Equal(expectedResult, result);
      Assert.Empty(errors);
    }

    [Fact]
    public void CheckRequiredDotNetVersionWrongMinVersionTest()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new CheckRequiredDotNetVersion();
      checkRequiredDotNetVersion.BuildEngine = buildEngine.Object;
      checkRequiredDotNetVersion.RepositoryRoot = repositoryRoot;
      checkRequiredDotNetVersion.SdkVersion = "6.0.101";
      // Act
      var result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
    }

    [Fact]
    public void CheckRequiredDotNetVersionNoFileTest()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new CheckRequiredDotNetVersion();
      checkRequiredDotNetVersion.BuildEngine = buildEngine.Object;
      checkRequiredDotNetVersion.RepositoryRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      checkRequiredDotNetVersion.SdkVersion = "7.0.200";
      // Act
      var result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
    }

    [Fact]
    public void CheckRequiredDotNetVersionInvalidVersion()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new CheckRequiredDotNetVersion();
      checkRequiredDotNetVersion.BuildEngine = buildEngine.Object;
      checkRequiredDotNetVersion.RepositoryRoot = repositoryRoot;
      checkRequiredDotNetVersion.SdkVersion = "7.0.a";
      // Act
      var result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
    }
  }
}
