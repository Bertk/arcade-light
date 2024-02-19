// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  /// <summary>Wraps RunAsync for ConditionalFact.</summary>
  public class SkippedFactTestCase : XunitTestCase
  {
#pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
    public SkippedFactTestCase() { }

    public SkippedFactTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod, object[] testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments) { }

    public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                    IMessageBus messageBus,
                                                    object[] constructorArguments,
                                                    ExceptionAggregator aggregator,
                                                    CancellationTokenSource cancellationTokenSource)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
      SkippedTestMessageBus skipMessageBus = new(messageBus);
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
      RunSummary result = await base.RunAsync(diagnosticMessageSink, skipMessageBus, constructorArguments, aggregator, cancellationTokenSource);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
      if (skipMessageBus.SkippedTestCount > 0)
      {
        result.Failed -= skipMessageBus.SkippedTestCount;
        result.Skipped += skipMessageBus.SkippedTestCount;
      }

      return result;
    }
  }
}
