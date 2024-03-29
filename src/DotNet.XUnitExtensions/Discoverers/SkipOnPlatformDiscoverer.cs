// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  public class SkipOnPlatformDiscoverer : ITraitDiscoverer
  {
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
      TestPlatforms testPlatforms = (TestPlatforms)0;

      // First argument is either the TestPlatform or the test platform to skip the test on.
#pragma warning disable CA1062
      if (traitAttribute.GetConstructorArguments().FirstOrDefault() is TestPlatforms tp)
      {
        testPlatforms = tp;
      }
#pragma warning restore CA1062
      if (DiscovererHelpers.TestPlatformApplies(testPlatforms))
      {
        return new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };
      }

      return Array.Empty<KeyValuePair<string, string>>();
    }
  }
}
