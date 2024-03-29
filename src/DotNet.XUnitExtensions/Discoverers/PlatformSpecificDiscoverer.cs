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
  /// <summary>
  /// This class discovers all of the tests and test classes that have
  /// applied the PlatformSpecific attribute
  /// </summary>
  public class PlatformSpecificDiscoverer : ITraitDiscoverer
  {
    /// <summary>
    /// Gets the trait values from the Category attribute.
    /// </summary>
    /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
    /// <returns>The trait values.</returns>
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
#pragma warning disable CA1062 // Validate arguments of public methods
      TestPlatforms platforms = (TestPlatforms)traitAttribute.GetConstructorArguments().First();
#pragma warning restore CA1062 // Validate arguments of public methods

      return DiscovererHelpers.TestPlatformApplies(platforms) ?
          Array.Empty<KeyValuePair<string, string>>() :
          new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) };
    }
  }
}
