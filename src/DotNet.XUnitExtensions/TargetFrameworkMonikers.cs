// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Xunit
{
  [Flags]
#pragma warning disable CA2217 // Do not mark enums with FlagsAttribute
  public enum TargetFrameworkMonikers
#pragma warning restore CA2217 // Do not mark enums with FlagsAttribute
  {
    Netcoreapp = 1,
    NetFramework = 2,
    Any = ~0
  }
}
