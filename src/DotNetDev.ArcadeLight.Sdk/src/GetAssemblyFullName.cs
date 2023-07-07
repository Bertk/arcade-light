// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Sdk
{
    public class GetAssemblyFullName : Microsoft.Build.Utilities.Task
    {
        [Required]
        public ITaskItem[] Items { get; set; }

        public string PathMetadata { get; set; }

        [Required]
        public string FullNameMetadata { get; set; }

        [Output]
        public ITaskItem[] ItemsWithFullName { get; set; }

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

