// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Xunit
{
  [XunitTestCaseDiscoverer("DotNet.XUnitExtensions.ConditionalTheoryDiscoverer", "DotNet.XUnitExtensions")]
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public sealed class ConditionalTheoryAttribute : TheoryAttribute
  {
    public Type CalleeType { get; private set; }
    public string[] ConditionMemberNames { get; private set; }

    public ConditionalTheoryAttribute(
            Type calleeType,
        params string[] conditionMemberNames)
    {
      CalleeType = calleeType;
      ConditionMemberNames = conditionMemberNames;
    }

    public ConditionalTheoryAttribute(params string[] conditionMemberNames)
    {
      ConditionMemberNames = conditionMemberNames;
    }
  }
}
