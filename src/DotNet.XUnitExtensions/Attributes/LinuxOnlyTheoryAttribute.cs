// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using DotNet.XUnitExtensions;

namespace Xunit
{
  /// <summary>
  /// This test should be run only on Windows.
  /// </summary>
  public sealed class LinuxOnlyTheoryAttribute : TheoryAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LinuxOnlyTheoryAttribute"/> class.
    /// </summary>
    /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
    public LinuxOnlyTheoryAttribute(string? additionalMessage = null)
    {
      if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        this.Skip = "This test requires Linux to run.".AppendAdditionalMessage(additionalMessage);
      }
    }

    public string? AdditionalMessage { get; }
  }
}
