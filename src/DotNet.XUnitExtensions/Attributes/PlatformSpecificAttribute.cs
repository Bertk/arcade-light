// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  /// <summary>
  /// Apply this attribute to your test method to specify this is a platform specific test.
  /// </summary>
  [TraitDiscoverer("DotNet.XUnitExtensions.PlatformSpecificDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
  public sealed class PlatformSpecificAttribute : Attribute, ITraitAttribute
  {
    public PlatformSpecificAttribute(TestPlatforms platforms) { }

    public TestPlatforms Platforms { get; }
  }
}
