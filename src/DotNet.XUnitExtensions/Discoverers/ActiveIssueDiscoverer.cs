// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions
{
  /// <summary>
  /// This class discovers all of the tests, test classes and test assemblies that have
  /// applied the ActiveIssue attribute
  /// </summary>
  public class ActiveIssueDiscoverer : ITraitDiscoverer
  {
    /// <summary>
    /// Gets the trait values from the Category attribute.
    /// </summary>
    /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
    /// <returns>The trait values.</returns>
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
#pragma warning disable CA1062
      IEnumerable<object> ctorArgs = traitAttribute.GetConstructorArguments();
#pragma warning restore CA1062
      return DiscovererHelpers.EvaluateArguments(ctorArgs, XunitConstants.Failing);
    }
  }
}
