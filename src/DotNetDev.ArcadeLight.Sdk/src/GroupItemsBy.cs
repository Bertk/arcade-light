// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Sdk
{
  /// <summary>
  /// Groups items by ItemSpec.
  /// 
  /// Given the following items:
  /// <![CDATA[
  /// <ItemGroup>
  ///   <Stuff Include="A" Value="X"/>
  ///   <Stuff Include="A" Value="Y"/>
  ///   <Stuff Include="B" Value="Z"/>
  /// </ItemGroup>
  /// ]]>
  /// 
  /// produces
  /// 
  /// <![CDATA[
  /// <ItemGroup>
  ///   <Stuff Include="A" Value="X;Y"/>
  ///   <Stuff Include="B" Value="Z"/>
  /// </ItemGroup>
  /// ]]>
  /// 
  /// </summary>
  public sealed class GroupItemsBy : Microsoft.Build.Utilities.Task
  {
    /// <summary>
    /// Items to group by their ItemSpec.
    /// </summary>
    [Required]
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] Items { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    /// <summary>
    /// Names of custom metadata to group (e.g. "Value" in the example above).
    /// When merging two items the values of metadata in this set are merged into a list, 
    /// while the first value is used for metadata not in this set.
    /// </summary>
    [Required]
#pragma warning disable CA1819 // Properties should not return arrays
    public string[] GroupMetadata { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    /// <summary>
    /// Items with grouped metadata values.
    /// </summary>
    [Output]
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] GroupedItems { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    public override bool Execute()
    {
      ITaskItem mergeItems(IEnumerable<ITaskItem> items)
      {
        ITaskItem result = items.First();

        foreach (ITaskItem item in items.Skip(1))
        {
          foreach (string metadataName in GroupMetadata)
          {
            string left = result.GetMetadata(metadataName);
            string right = item.GetMetadata(metadataName);

            if (string.IsNullOrEmpty(right))
            {
              result.SetMetadata(metadataName,
                            (string.IsNullOrEmpty(left) || left == right) ? right : left);
            }
            else
            {
              result.SetMetadata(metadataName,
                            (string.IsNullOrEmpty(left) || left == right) ? right : left + ";" + right);
            }
          }
        }

        return result;
      }

      GroupedItems = (from item in Items
                      group item by item.ItemSpec
                      into itemsWithEqualKey
                      select mergeItems(itemsWithEqualKey)).ToArray();

      return true;
    }
  }
}

