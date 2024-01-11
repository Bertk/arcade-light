// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using DotNetDev.ArcadeLight.Test.Common;
using Microsoft.Build.Utilities;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class GroupItemsByTests
  {
    private static readonly string[] expected = new[]
      {
                "A: X='A2.X' Y='A1.Y' Z='A1.Z;A2.Z' U='' W='A1.W'",
                "B: X='' Y='' Z='B1.Z' U='' W=''",
                "C: X='C1.X' Y='C2.Y' Z='C1.Z;C2.Z' U='' W=''",
            };

    [Fact]
    public void GroupItemsBy()
    {
      GroupItemsBy task = new GroupItemsBy()
      {
        Items = new TaskItem[]
                {
                    new TaskItem("A", new Dictionary<string, string> { { "Y", "A1.Y" }, { "Z", "A1.Z" }, { "W", "A1.W" } }),
                    new TaskItem("B", new Dictionary<string, string> { { "Z", "B1.Z" } }),
                    new TaskItem("A", new Dictionary<string, string> { { "X", "A2.X" }, { "Z", "A2.Z" }, { "W", "A2.W" } }),
                    new TaskItem("C", new Dictionary<string, string> { { "X", "C1.X" }, { "Z", "C1.Z" } }),
                    new TaskItem("C", new Dictionary<string, string> { { "Y", "C2.Y" }, { "Z", "C2.Z" } }),
                },
        GroupMetadata = new[] { "X", "Y", "Z", "U" }
      };

      bool result = task.Execute();
      string[] inspectMetadata = new[] { "X", "Y", "Z", "U", "W" };

      AssertEx.Equal(expected, task.GroupedItems.Select(i => $"{i.ItemSpec}: {string.Join(" ", inspectMetadata.Select(m => $"{m}='{i.GetMetadata(m)}'"))}"));

      Assert.True(result);
    }
  }
}
