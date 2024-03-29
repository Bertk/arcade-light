// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using DotNetDev.ArcadeLight.Test.Common;
using Microsoft.Build.Utilities;
using Xunit;

namespace DotNetDev.ArcadeLight.Sdk.Tests
{
  public class GetAssemblyFullNameTests
  {
    [Fact]
    public void PathInMetadata()
    {
      System.Reflection.Assembly objectAssembly = typeof(object).Assembly;
      System.Reflection.Assembly thisAssembly = typeof(GetAssemblyFullNameTests).Assembly;

      GetAssemblyFullName task = new GetAssemblyFullName()
      {
        Items = new TaskItem[]
                {
                    new TaskItem("Item", new Dictionary<string, string> { { "SomePath", objectAssembly.Location } }),
                    new TaskItem("Item", new Dictionary<string, string> { { "SomePath", thisAssembly.Location } }),
                },
        PathMetadata = "SomePath",
        FullNameMetadata = "SomeFullName"
      };

      bool result = task.Execute();

      AssertEx.Equal(new[]
     {
                objectAssembly.FullName,
                thisAssembly.FullName
            }, task.ItemsWithFullName.Select(i => i.GetMetadata("SomeFullName")));

      Assert.True(result);
    }

    [Fact]
    public void PathInItemSpec()
    {
      System.Reflection.Assembly objectAssembly = typeof(object).Assembly;
      System.Reflection.Assembly thisAssembly = typeof(GetAssemblyFullNameTests).Assembly;

      GetAssemblyFullName task = new GetAssemblyFullName()
      {
        Items = new TaskItem[]
                {
                    new TaskItem(objectAssembly.Location),
                    new TaskItem(thisAssembly.Location),
                },
        FullNameMetadata = "SomeFullName"
      };

      bool result = task.Execute();

      AssertEx.Equal(new[]
      {
                objectAssembly.FullName,
                thisAssembly.FullName
            }, task.ItemsWithFullName.Select(i => i.GetMetadata("SomeFullName")));

      Assert.True(result);
    }
  }
}
