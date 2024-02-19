// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;

namespace DotNet.XUnitExtensions
{
  /// <summary>
  /// Trace Listener for corefx Desktop test execution to avoid showing assert pop-ups and making the test fail when an Assert fails.
  /// </summary>
  public class DesktopTestTraceListener : DefaultTraceListener
  {

#pragma warning disable CS0419 // Ambiguous reference in cref attribute
    /// <summary>
    /// Override of <see cref="DefaultTraceListener.Fail" /> to handle Assert failures with custom behavior.
    /// When an Assert failure happens during test execution we will rather throw a DebugAssertException so that the test fails and we have a full StackTrace.
    /// </summary>
    public override void Fail(string message, string detailMessage)
#pragma warning restore CS0419 // Ambiguous reference in cref attribute
    {
      throw new DebugAssertException(message, detailMessage);
    }

#pragma warning disable CA1064 // Exceptions should be public
    private sealed class DebugAssertException : Exception
#pragma warning restore CA1064 // Exceptions should be public
    {
      internal DebugAssertException(string message, string detailMessage) :
          base(message + Environment.NewLine + detailMessage)
      {
      }

      public DebugAssertException()
      {
      }

      public DebugAssertException(string message) : base(message)
      {
      }

      public DebugAssertException(string message, Exception innerException) : base(message, innerException)
      {
      }
    }
  }
}
