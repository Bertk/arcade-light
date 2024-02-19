// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace DotNet.XUnitExtensions
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
  public struct XunitConstants
#pragma warning restore CA1815 // Override equals and operator equals on value types
  {
    internal const string Failing = "failing";
    internal const string OuterLoop = "outerloop";

    public const string Category = "category";
    public const string IgnoreForCI = "ignoreforci";
    public const string RequiresElevation = "requireselevation";
  }
}
