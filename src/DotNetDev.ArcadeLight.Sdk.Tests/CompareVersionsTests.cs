using System.Collections.Generic;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class CompareVersionsTests
  {
    private readonly Mock<IBuildEngine> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;


    public CompareVersionsTests()
    {
      buildEngine = new Mock<IBuildEngine>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

    }


    [Theory]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("1.0.0", "2.0.0", -1)]
    [InlineData("1.2.0", "1.0.0", 1)]
    public void CompareVersionsTaskVerify(string left, string right, int result)
    {
      // Arrange
      CompareVersions compareVersionsTask = new CompareVersions();
      compareVersionsTask.BuildEngine = buildEngine.Object;
      compareVersionsTask.Left = left;
      compareVersionsTask.Right = right;

      // Act
      bool currentResult = compareVersionsTask.Execute();


      // Assert
      Assert.Equal(result, compareVersionsTask.Result);
      Assert.Empty(errors);
    }

    [Theory]
    [InlineData("1.2.b", "1.0.0", 0)]
    [InlineData("1.2.0", "1.0.c", 0)]
    public void CompareVersionsTaskInvalidVersion(string left, string right, int result)
    {
      // Arrange
      CompareVersions compareVersionsTask = new CompareVersions();
      compareVersionsTask.BuildEngine = buildEngine.Object;
      compareVersionsTask.Left = left;
      compareVersionsTask.Right = right;

      // Act
      bool currentResult = compareVersionsTask.Execute();


      // Assert
      Assert.Equal(result, compareVersionsTask.Result);
      Assert.NotEmpty(errors);
    }
  }
}
