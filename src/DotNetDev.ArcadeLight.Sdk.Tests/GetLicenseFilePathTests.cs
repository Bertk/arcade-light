// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class GetLicenseFilePathTests
  {
    [Theory]
    [InlineData("licenSe.TXT")]
    [InlineData("license.md")]
    [InlineData("LICENSE")]
    public void GetLicenseFilePath(string licenseFileName)
    {
      string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
      Directory.CreateDirectory(dir);
      string licensePath = Path.Combine(dir, licenseFileName);

      File.WriteAllText(licensePath, "");

      GetLicenseFilePath task = new GetLicenseFilePath()
      {
        Directory = dir
      };

      bool result = task.Execute();
      Assert.Equal(licensePath, task.Path);
      Assert.True(result);

      Directory.Delete(dir, recursive: true);
    }
  }
}
