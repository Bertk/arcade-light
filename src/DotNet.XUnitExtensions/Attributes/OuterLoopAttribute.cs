// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Sdk;

namespace Xunit
{
  /// <summary>
  /// Apply this attribute to your test method to specify a outer-loop category.
  /// </summary>
  [TraitDiscoverer("DotNet.XUnitExtensions.OuterLoopTestsDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
  public sealed class OuterLoopAttribute : Attribute, ITraitAttribute
  {
    public Type CalleeType { get; private set; }
    public string[] ConditionMemberNames { get; private set; }

    public OuterLoopAttribute() { }
    public OuterLoopAttribute(string reason) { }
    public OuterLoopAttribute(string reason, TestPlatforms platforms) { }
    public OuterLoopAttribute(string reason, TargetFrameworkMonikers framework) { }
    public OuterLoopAttribute(string reason, TestRuntimes runtime) { }
    public OuterLoopAttribute(string reason, TestPlatforms platforms, TargetFrameworkMonikers framework, TestRuntimes runtime = TestRuntimes.Any) { }
    public OuterLoopAttribute(string reason, Type calleeType, params string[] conditionMemberNames)
    {
      CalleeType = calleeType;
      ConditionMemberNames = conditionMemberNames;
    }

    public string Reason { get; }
    public TestPlatforms? Platforms { get; }
    public TargetFrameworkMonikers? Framework { get; }
    public TestRuntimes? Runtime { get; }
  }
}
