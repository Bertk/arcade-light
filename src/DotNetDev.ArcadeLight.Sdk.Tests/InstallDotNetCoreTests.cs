using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Moq;
using Xunit;
using Xunit.v3;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class InstallDotNetCoreTests
  {
    private readonly Mock<IBuildEngine4> buildEngine;
    private readonly List<BuildErrorEventArgs> errors;
    private readonly string projectRootDir;
    private readonly string repositoryEngineeringDir;

    public enum SupportedOS
    {
      FreeBSD = 1,
      Linux = 2,
      macOS = 3,
      Windows = 4,
    }

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
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.True(success);
      Assert.Empty(errors);
    }

    [Fact]
    public void InstallDotNetCoreNoGlobalJsonFile()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      // returns success and creates a warning
      Assert.True(success);
      // warning list is not available and errors are empty
      Assert.Empty(errors);

    }

    [Fact]
    public void InstallDotNetCoreNoInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.GetFullPath(Path.Combine(projectRootDir, "global.json")),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"xxx.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.False(success);
      Assert.NotEmpty(errors);
      Assert.Contains("Unable to find dotnet install script", errors[0].Message);
    }
    [Fact]
    [SupportedOS(SupportedOS.Windows)]
    public void TryInstallDotNetRuntimeWithInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.Combine(projectRootDir, @"src/DotNetDev.ArcadeLight.Sdk.Tests/testassets/installVersion/global.json"),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.True(success);
      Assert.Empty(errors);
    }

    [Fact]
    [SupportedOS(SupportedOS.Windows)]
    public void FailInstallDotNetRuntimeWithInstallScript()
    {
      //Arrange
      InstallDotNetCore customTask = new()
      {
        GlobalJsonPath = Path.Combine(projectRootDir, @"src/DotNetDev.ArcadeLight.Sdk.Tests/testassets/failVersion/global.json"),
        DotNetInstallScript = Path.GetFullPath(Path.Combine(repositoryEngineeringDir, @"commonlight/dotnet-install.cmd")),
        BuildEngine = buildEngine.Object
      };
      //Act and Assert
      bool success = customTask.Execute();
      Assert.False(success);
      Assert.NotEmpty(errors);
      Assert.Contains("dotnet-install failed", errors[0].Message);
    }
    public sealed class SupportedOSAttribute(params SupportedOS[] supportedOSes) :
        BeforeAfterTestAttribute
    {
      private static readonly Dictionary<SupportedOS, OSPlatform> osMappings = new()
    {
        { SupportedOS.FreeBSD, OSPlatform.Create("FreeBSD") },
        { SupportedOS.Linux, OSPlatform.Linux },
        { SupportedOS.macOS, OSPlatform.OSX },
        { SupportedOS.Windows, OSPlatform.Windows },
    };

      public override ValueTask Before(MethodInfo methodUnderTest, IXunitTest test)
      {
        var match = false;

        foreach (var supportedOS in supportedOSes)
        {
          if (!osMappings.TryGetValue(supportedOS, out var osPlatform))
            throw new ArgumentException($"Supported OS value '{supportedOS}' is not a known OS", nameof(supportedOSes));

          if (RuntimeInformation.IsOSPlatform(osPlatform))
          {
            match = true;
            break;
          }
        }

        // We use the dynamic skip exception message pattern to turn this into a skipped test
        // when it's not running on one of the targeted OSes
        if (!match)
          throw new Exception($"$XunitDynamicSkip$This test is not supported on {RuntimeInformation.OSDescription}");

        return default;
      }
    }
  }
}
