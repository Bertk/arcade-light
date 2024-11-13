using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Moq;
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
    [InlineData("8.0.404", true)]
    [InlineData("8.0.888", true)]
    public void CheckRequiredDotNetVersionVerify(string minSdkVersionStr, bool expectedResult)
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = repositoryRoot,
        SdkVersion = minSdkVersionStr
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.Equal(expectedResult, result);
      Assert.Empty(errors);
    }

    [Fact]
    public void CheckRequiredDotNetVersionWrongMinVersionTest()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = repositoryRoot,
        SdkVersion = "6.0.101"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
      Assert.Contains("is below the minimum required version", errors[0].Message);
    }

    [Fact]
    public void CheckRequiredDotNetVersionNoFileTest()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        SdkVersion = "7.0.200"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
      Assert.Contains("Error reading file", errors[0].Message);
    }

    [Fact]
    public void CheckRequiredDotNetVersionInvalidVersion()
    {
      // Arrange
      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = repositoryRoot,
        SdkVersion = "7.0.a"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
      Assert.Equal("Invalid version: 7.0.a", errors[0].Message);
    }

    [WindowsOnlyFact]
    public void CheckRequiredDotNetVersionInvalidFile()
    {
      // Arrange

      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = ":",
        SdkVersion = "7.0.100"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.Contains("Error accessing file", errors[0].Message);
    }


    [WindowsOnlyFact]
    public void CheckRequiredDotNetVersionInvalidGlobalFileContent()
    {

      // Arrange

      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = Path.Combine(repositoryRoot, "src\\DotNetDev.ArcadeLight.Sdk.Tests\\testassets\\invalidcontent"),
        SdkVersion = "7.0.100"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
      Assert.Contains("Unable to determine dotnet version from file", errors[0].Message);
    }

    [WindowsOnlyFact]
    public void CheckRequiredDotNetVersionInvalidDotnetVersion()
    {

      // Arrange

      CheckRequiredDotNetVersion checkRequiredDotNetVersion = new()
      {
        BuildEngine = buildEngine.Object,
        RepositoryRoot = Path.Combine(repositoryRoot, "src\\DotNetDev.ArcadeLight.Sdk.Tests\\testassets\\invalidVersion"),
        SdkVersion = "7.0.100"
      };
      // Act
      bool result = checkRequiredDotNetVersion.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
      Assert.Contains("DotNet version specified in", errors[0].Message);

    }
  }
}
