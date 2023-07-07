// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using Xunit.Sdk;

namespace DotNet.XUnitExtensions.Tests
{
  [TestCaseOrderer("DotNet.XUnitExtensions.Tests.AlphabeticalOrderer", "DotNet.XUnitExtensions.Tests")]
  public class ConditionalAttributeTests
  {
    // The tests under this class are to validate that ConditionalFact and ConditionalAttribute tests are being executed correctly
    // In the past we have had cases where infrastructure changes broke this feature and tests where not running in certain framework.
    // This test class is test order dependent so do not rename the tests.
    // If new tests need to be added, follow the same naming pattern ConditionalAttribute{LetterToOrderTest} and then add a Validate{TestName}.

    private static bool s_conditionalFactExecuted;
    private static int s_conditionalTheoryCount;

    public static bool AlwaysTrue => true;

    [ConditionalFact(nameof(AlwaysTrue))]
#pragma warning disable S2699 // Tests should include assertions
    public void ConditionalAttributeA()
#pragma warning restore S2699 // Tests should include assertions
    {
      s_conditionalFactExecuted = true;
    }

    [Fact]
    [OuterLoop("never outer loop", TestPlatforms.Any & ~TestPlatforms.Any)]
    public void NeverConditionalOuterLoopAttribute()
    {
      System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
      System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<string, string>> res = TraitHelper.GetTraits(method);

      Assert.Empty(res);
    }

    [Fact]
    [OuterLoop("always outer loop", TestPlatforms.Any)]
    public void AlwaysConditionalOuterLoopAttribute()
    {
      System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
      System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<string, string>> res = TraitHelper.GetTraits(method);

      Assert.Single(res);
      Assert.Equal("outerloop", res[0].Value);
    }

    [ConditionalTheory(nameof(AlwaysTrue))]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
#pragma warning disable S2699 // Tests should include assertions
    public void ConditionalAttributeB(int _)
#pragma warning restore S2699 // Tests should include assertions
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
      s_conditionalTheoryCount++;
    }

    [Fact]
    public void ValidateConditionalFact()
    {
      Assert.True(s_conditionalFactExecuted);
    }

    [Fact]
    public void ValidateConditionalTheory()
    {
      Assert.Equal(3, s_conditionalTheoryCount);
    }
  }
}
