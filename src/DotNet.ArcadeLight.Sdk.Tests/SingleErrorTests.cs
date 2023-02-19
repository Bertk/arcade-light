using Microsoft.Build.Framework;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotNet.ArcadeLight.Sdk.Tests
{
  public class SingleErrorTests
  {
    private readonly MockRepository mockRepository;
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;


    public SingleErrorTests()
    {
      mockRepository = new MockRepository(MockBehavior.Loose);
      buildEngine = new Mock<IBuildEngine4>(MockBehavior.Loose);
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

    }

    [Fact]
    public void SingleErrorSuccess()
    {
      // Arrange
      var singleError = new SingleError();
      singleError.BuildEngine = buildEngine.Object;
      singleError.Text = "nasty error message";

      // Act
      var result = singleError.Execute();

      // Assert
      Assert.False(result);
      mockRepository.VerifyAll();
      Assert.NotEmpty(errors);
    }
  }
}
