// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Sdk
{
  public class GetAssemblyFullName : Microsoft.Build.Utilities.Task
  {
    [Required]
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] Items { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public string PathMetadata { get; set; }

    [Required]
    public string FullNameMetadata { get; set; }

    [Output]
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] ItemsWithFullName { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public override bool Execute()
    {
      ItemsWithFullName = Items;

      foreach (ITaskItem item in Items)
      {
        string assemblyPath = string.IsNullOrEmpty(PathMetadata) ? item.ItemSpec : item.GetMetadata(PathMetadata);
        item.SetMetadata(FullNameMetadata, AssemblyName.GetAssemblyName(assemblyPath).FullName);
      }

      return true;
    }
  }
}

