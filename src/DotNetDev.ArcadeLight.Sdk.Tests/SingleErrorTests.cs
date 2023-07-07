using System.Collections.Generic;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class SingleErrorTests
  {
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;


    public SingleErrorTests()
    {
      buildEngine = new Mock<IBuildEngine4>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

    }

    [Fact]
    public void SingleErrorVerify()
    {
      // Arrange
      SingleError singleError = new SingleError();
      singleError.BuildEngine = buildEngine.Object;
      singleError.Text = "nasty error message";

      // Act
      bool result = singleError.Execute();

      // Assert
      Assert.False(result);
      Assert.NotEmpty(errors);
    }
  }
}
