// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Xunit
{
  [Flags]
#pragma warning disable CA2217 // Do not mark enums with FlagsAttribute
  public enum RuntimeConfiguration
#pragma warning restore CA2217 // Do not mark enums with FlagsAttribute
  {
    Any = ~0,
    Checked = 1,
    Debug = 1 << 1,
    Release = 1 << 2
  }
}
