using System.Collections.Generic;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class DownloadFileTests
  {
    private MockRepository mockRepository;
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;
    private readonly string repositoryRoot;


    public DownloadFileTests()
    {

      this.mockRepository = new MockRepository(MockBehavior.Strict);

    }

    private DownloadFile CreateDownloadFile()
    {

      DownloadFile _DownloadFile = new()
      {
        BuildEngine = buildEngine.Object,
        DestinationPath = "/temp"
        //Uri = minSdkVersionStr
      };
      return _DownloadFile;
    }

    [Fact]
    public void Cancel_StateUnderTest_ExpectedBehavior()
    {
      // Arrange
      var downloadFile = this.CreateDownloadFile();

      // Act
      downloadFile.Cancel();

      // Assert
      Assert.True(false);
      this.mockRepository.VerifyAll();
    }

    [Fact]
    public void Execute_StateUnderTest_ExpectedBehavior()
    {
      // Arrange
      var downloadFile = this.CreateDownloadFile();

      // Act
      var result = downloadFile.Execute();

      // Assert
      Assert.True(false);
      this.mockRepository.VerifyAll();
    }

    [Fact]
    public void Dispose_StateUnderTest_ExpectedBehavior()
    {
      // Arrange
      var downloadFile = this.CreateDownloadFile();

      // Act
      downloadFile.Dispose();

      // Assert
      Assert.True(false);
      this.mockRepository.VerifyAll();
    }
  }
}
