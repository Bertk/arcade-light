// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  public class SkipTestException : Exception
  {
    public SkipTestException(string reason)
        : base(reason) { }

    public SkipTestException()
    {
    }

    public SkipTestException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }

  /// <summary>Implements message bus to communicate tests skipped via SkipTestException.</summary>
  public class SkippedTestMessageBus : IMessageBus
  {
    readonly IMessageBus innerBus;

    public SkippedTestMessageBus(IMessageBus innerBus)
    {
      this.innerBus = innerBus;
    }

    public int SkippedTestCount { get; private set; }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public bool QueueMessage(IMessageSinkMessage message)
    {
      ITestFailed testFailed = message as ITestFailed;

      if (testFailed != null)
      {
        string exceptionType = testFailed.ExceptionTypes.FirstOrDefault();
        if (exceptionType == typeof(SkipTestException).FullName)
        {
          SkippedTestCount++;
          return innerBus.QueueMessage(new TestSkipped(testFailed.Test, testFailed.Messages.FirstOrDefault()));
        }
      }

      // Nothing we care about, send it on its way
      return innerBus.QueueMessage(message);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Cleanup
    }
  }
}
