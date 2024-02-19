// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  public class ConditionalFactDiscoverer : FactDiscoverer
  {
    public ConditionalFactDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink) { }

    protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
    {
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA2000 // Dispose objects before losing scope
      if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, factAttribute.GetConstructorArguments().ToArray(), out string skipReason, out ExecutionErrorTestCase errorTestCase))
      {
        return skipReason != null
            ? (IXunitTestCase)new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod)
            : new SkippedFactTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod); // Test case skippable at runtime.
      }
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore CA1062 // Validate arguments of public methods

      return errorTestCase;
    }
  }
}
