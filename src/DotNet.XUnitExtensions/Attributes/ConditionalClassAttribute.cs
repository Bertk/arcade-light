// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Xunit
{
  [TraitDiscoverer("DotNet.XUnitExtensions.ConditionalClassDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class ConditionalClassAttribute : Attribute, ITraitAttribute
  {
    public Type CalleeType { get; private set; }
    public string[] ConditionMemberNames { get; private set; }

    public ConditionalClassAttribute(
            Type calleeType,
        params string[] conditionMemberNames)
    {
      CalleeType = calleeType;
      ConditionMemberNames = conditionMemberNames;
    }
  }
}
