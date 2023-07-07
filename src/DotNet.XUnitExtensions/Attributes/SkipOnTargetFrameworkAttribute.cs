// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  /// <summary>
  /// Apply this attribute to your test method to specify this is a platform specific test.
  /// </summary>
  [TraitDiscoverer("DotNet.XUnitExtensions.SkipOnTargetFrameworkDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
  public class SkipOnTargetFrameworkAttribute : Attribute, ITraitAttribute
  {
    public SkipOnTargetFrameworkAttribute(TargetFrameworkMonikers platform, string reason = null) { }
  }
}
