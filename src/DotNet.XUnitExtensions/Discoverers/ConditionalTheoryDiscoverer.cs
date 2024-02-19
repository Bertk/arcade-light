// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  public class ConditionalTheoryDiscoverer : TheoryDiscoverer
  {
    private readonly Dictionary<IMethodInfo, string> _conditionCache;

    public ConditionalTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
    {
      _conditionCache = new Dictionary<IMethodInfo, string>();
    }

    protected override IEnumerable<IXunitTestCase> CreateTestCasesForTheory(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
    {
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA2000 // Dispose objects before losing scope
      if (ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, theoryAttribute.GetConstructorArguments().ToArray(), out string skipReason, out ExecutionErrorTestCase errorTestCase))
      {
        return skipReason != null
           ? new[] { new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod) }
           : new IXunitTestCase[] { new SkippedTheoryTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod) }; // Theory skippable at runtime.
      }
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore CA1062 // Validate arguments of public methods

      return new IXunitTestCase[] { errorTestCase };
    }

    protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
    {
#pragma warning disable CA1062
      IMethodInfo methodInfo = testMethod.Method;
#pragma warning restore CA1062
      List<IXunitTestCase> skippedTestCase = new List<IXunitTestCase>();

      if (!_conditionCache.TryGetValue(methodInfo, out string skipReason))
      {
#pragma warning disable CA1062
#pragma warning disable CA2000 // Dispose objects before losing scope
        if (!ConditionalTestDiscoverer.TryEvaluateSkipConditions(discoveryOptions, DiagnosticMessageSink, testMethod, theoryAttribute.GetConstructorArguments().ToArray(), out skipReason, out ExecutionErrorTestCase errorTestCase))
        {
          return new IXunitTestCase[] { errorTestCase };
        }
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore CA1062
        _conditionCache.Add(methodInfo, skipReason);

        if (skipReason != null)
        {
          // If this is the first time we evaluate the condition we return a SkippedTestCase to avoid printing a skip for every inline-data.
          skippedTestCase.Add(new SkippedTestCase(skipReason, DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod));
        }
      }

      return skipReason != null ?
                  (IEnumerable<IXunitTestCase>)skippedTestCase
                  : new[] { new SkippedFactTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, dataRow) }; // Test case skippable at runtime.
    }
  }
}
