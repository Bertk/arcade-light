// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  [TraitDiscoverer("DotNet.XUnitExtensions.SkipOnPlatformDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public sealed class SkipOnPlatformAttribute : Attribute, ITraitAttribute
  {
    internal SkipOnPlatformAttribute() { }
    public SkipOnPlatformAttribute(TestPlatforms testPlatforms, string reason) { }

    public TestPlatforms TestPlatforms { get; }
    public string Reason { get; }
  }
}
