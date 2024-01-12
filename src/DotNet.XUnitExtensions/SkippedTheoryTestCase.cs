// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  /// <summary>Wraps RunAsync for ConditionalTheory.</summary>
  public class SkippedTheoryTestCase : XunitTheoryTestCase
  {
#pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
    public SkippedTheoryTestCase() { }

    public SkippedTheoryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod) { }

    public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                    IMessageBus messageBus,
                                                    object[] constructorArguments,
                                                    ExceptionAggregator aggregator,
                                                    CancellationTokenSource cancellationTokenSource)
    {
      using SkippedTestMessageBus skipMessageBus = new(messageBus);
      var result = await base.RunAsync(diagnosticMessageSink, skipMessageBus, constructorArguments, aggregator, cancellationTokenSource).ConfigureAwait(false);
      if (skipMessageBus.SkippedTestCount > 0)
      {
        result.Failed -= skipMessageBus.SkippedTestCount;
        result.Skipped += skipMessageBus.SkippedTestCount;
      }

      return result;
    }
  }
}
