using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Moq;
using Xunit;
using static DotNetDev.ArcadeLight.Sdk.Tests.InstallDotNetCoreTests;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class LocateDotNetTests
  {
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;
    private readonly string repositoryRoot;

    public LocateDotNetTests()
    {
      buildEngine = new Mock<IBuildEngine4>();
      errors = new List<BuildErrorEventArgs>();
      buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));

      repositoryRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../../../../"));
    }

    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [Fact]
    public void LocateDotNetVerify()
    {
      Assert.SkipUnless(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test requires Windows");
      // Arrange
      LocateDotNet locateDotNet = new LocateDotNet();
      locateDotNet.BuildEngine = buildEngine.Object;
      locateDotNet.RepositoryRoot = repositoryRoot;

      // Act
      locateDotNet.Execute();

      // Assert
      Assert.NotNull(locateDotNet.DotNetPath);
      Assert.True(File.Exists(locateDotNet.DotNetPath));
      Assert.Empty(errors);
    }

    [Fact]
    public void LocateDotNetWrongFolder()
    {
      // Arrange
      LocateDotNet locateDotNet = new LocateDotNet();
      locateDotNet.BuildEngine = buildEngine.Object;
      locateDotNet.RepositoryRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      // Act & Assert
      Assert.Throws<FileNotFoundException>(() => locateDotNet.Execute());
    }
  }
}
