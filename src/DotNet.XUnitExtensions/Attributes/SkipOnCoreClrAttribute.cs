// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  [TraitDiscoverer("DotNet.XUnitExtensions.SkipOnCoreClrDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public sealed class SkipOnCoreClrAttribute : Attribute, ITraitAttribute
  {
    internal SkipOnCoreClrAttribute() { }

    public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms) { }
    public SkipOnCoreClrAttribute(string reason, RuntimeTestModes testMode) { }
    public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations) { }
    public SkipOnCoreClrAttribute(string reason, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes) { }
    public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations) { }
    public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeTestModes testMode) { }
    public SkipOnCoreClrAttribute(string reason, TestPlatforms testPlatforms, RuntimeConfiguration runtimeConfigurations, RuntimeTestModes testModes) { }
    public SkipOnCoreClrAttribute(string reason) { }

    public string Reason { get; }
    public TestPlatforms? TestPlatforms { get; }
    public RuntimeTestModes? TestMode { get; }
    public RuntimeConfiguration? RuntimeConfigurations { get; }
    public RuntimeTestModes? TestModes { get; }
  }
}
