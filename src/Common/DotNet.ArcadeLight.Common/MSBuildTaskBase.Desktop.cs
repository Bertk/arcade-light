// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using DotNet.ArcadeLight.Common.Desktop;

namespace DotNet.ArcadeLight.Common
{
    public partial class MSBuildTaskBase
    {
        static MSBuildTaskBase()
        {
            AssemblyResolver.Enable();
        }
    }
}
