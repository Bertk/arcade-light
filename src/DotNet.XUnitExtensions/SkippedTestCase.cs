// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  /// <summary>Wraps another test case that should be skipped.</summary>
  internal sealed class SkippedTestCase : XunitTestCase
  {
    private string _skipReason;

#pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
#pragma warning restore S1133 // Deprecated code should be removed
    public SkippedTestCase() : base()
    {
    }

    public SkippedTestCase(
        string skipReason,
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod,
        object[] testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
      _skipReason = skipReason;
    }

    protected override string GetSkipReason(IAttributeInfo factAttribute)
        => _skipReason ?? base.GetSkipReason(factAttribute);

    public override void Deserialize(IXunitSerializationInfo data)
    {
      base.Deserialize(data);
      _skipReason = data.GetValue<string>(nameof(_skipReason));
    }

    public override void Serialize(IXunitSerializationInfo data)
    {
      base.Serialize(data);
      data.AddValue(nameof(_skipReason), _skipReason);
    }
  }
}
