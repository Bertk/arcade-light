// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  [TraitDiscoverer("DotNet.XUnitExtensions.SkipOnMonoDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class SkipOnMonoAttribute : Attribute, ITraitAttribute
  {
    internal SkipOnMonoAttribute() { }
    public SkipOnMonoAttribute(string reason, TestPlatforms testPlatforms = TestPlatforms.Any) { }

    public string Reason { get; }
    public TestPlatforms TestPlatforms { get; }
  }
}
