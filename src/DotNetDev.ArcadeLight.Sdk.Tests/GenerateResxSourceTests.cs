// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using DotNetDev.ArcadeLight.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
    public class GenerateResxSourceTests
    {
        private readonly ITestOutputHelper _output;

        public GenerateResxSourceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(true, false, false, "TestStrings.EmitFormatMethods.cs.txt")]
        [InlineData(false, false, false, "TestStrings.Default.cs.txt")]
        [InlineData(false, true, true, "TestStrings.AsConstants.cs.txt")]
        [InlineData(false, false, true, "TestStrings.OmitGetResourceString.cs.txt")]
        public void GeneratesCSharp(bool emitFormatMethods, bool asConstants, bool omitGetResourceString, string expectedFileName)
        {
      string resx = Path.Combine(AppContext.BaseDirectory, "testassets", "Resources", "TestStrings.resx");
      string actualFile = Path.Combine(AppContext.BaseDirectory, Path.GetRandomFileName());

      MockEngine engine = new MockEngine(_output);
      GenerateResxSource task = new GenerateResxSource
            {
                BuildEngine = engine,
                ResourceFile = resx,
                ResourceName = "Microsoft.DotNet.TestStrings",
                ResourceClassName = "Microsoft.DotNet.TestStrings",
                EmitFormatMethods = emitFormatMethods,
                AsConstants = asConstants,
                OmitGetResourceString = omitGetResourceString,
                Language = "C#",
                OutputPath = actualFile,
            };

      string expectedFile = Path.Combine(AppContext.BaseDirectory, "testassets", "Resources", expectedFileName);

            if (File.Exists(actualFile))
            {
                File.Delete(actualFile);
            }

            Assert.True(task.Execute(), "Task failed");

            Assert.Empty(engine.Warnings);

            Assert.True(File.Exists(actualFile), "Actual file does not exist");
      string actualFileContents = File.ReadAllText(actualFile);
            _output.WriteLine(actualFileContents);
            Assert.Equal(File.ReadAllText(expectedFile), actualFileContents, ignoreLineEndingDifferences: true);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("A", "A")]
        [InlineData("_A", "_A")]
        [InlineData(".A", "_A")]
        [InlineData("4A", "_4A")]
        [InlineData("4(.-)A", "_4____A")]
        [InlineData("A\u0660\u2040\u0601\u0300\u0903", "A\u0660\u2040\u0601\u0300\u0903")]
        public void GetIdentifierFromResourceName(string name, string expectedIdentifier)
        {
            Assert.Equal(expectedIdentifier, GenerateResxSource.GetIdentifierFromResourceName(name));
        }
    }
}
