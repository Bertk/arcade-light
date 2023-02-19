using Microsoft.Build.Framework;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotNet.ArcadeLight.Sdk.Tests
{
  public class CompareVersionsTests
  {
    private readonly MockRepository mockRepository;
    private readonly Mock<IBuildEngine> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;


    public CompareVersionsTests()
    {
      mockRepository = new MockRepository(MockBehavior.Strict);
      buildEngine = new Mock<IBuildEngine>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

    }


    [Theory]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("1.0.0", "2.0.0", -1)]
    [InlineData("1.2.0", "1.0.0", 1)]
    public void CompareVersionsTaskTest(string left, string right, int result)
    {
      // Arrange
      CompareVersions compareVersionsTask = new CompareVersions();
      compareVersionsTask.BuildEngine = buildEngine.Object;
      compareVersionsTask.Left = left;
      compareVersionsTask.Right = right;

      // Act
      var currentResult = compareVersionsTask.Execute();


      // Assert
      Assert.Equal(result, compareVersionsTask.Result);
      mockRepository.VerifyAll();
      Assert.Empty(errors);
    }

    [Theory]
    [InlineData("1.2.b", "1.0.0", 0)]
    [InlineData("1.2.0", "1.0.c", 0)]
    public void CompareVersionsTaskTestUnhappy(string left, string right, int result)
    {
      // Arrange
      CompareVersions compareVersionsTask = new CompareVersions();
      compareVersionsTask.BuildEngine = buildEngine.Object;
      compareVersionsTask.Left = left;
      compareVersionsTask.Right = right;

      // Act
      var currentResult = compareVersionsTask.Execute();


      // Assert
      Assert.Equal(result, compareVersionsTask.Result);
      mockRepository.VerifyAll();
      Assert.NotEmpty(errors);
    }
  }
}
