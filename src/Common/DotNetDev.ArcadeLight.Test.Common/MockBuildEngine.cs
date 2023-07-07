// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Test.Common
{
  public class MockBuildEngine : IBuildEngine
  {
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
    public bool ContinueOnError => throw new NotImplementedException();
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations

    public int LineNumberOfTaskNode => 0;

    public int ColumnNumberOfTaskNode => 0;

    public string ProjectFileOfTaskNode => "Fake File";

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA1051 // Do not declare visible instance fields
    public List<CustomBuildEventArgs> CustomBuildEvents = new();
    public List<BuildErrorEventArgs> BuildErrorEvents = new();
    public List<BuildMessageEventArgs> BuildMessageEvents = new();
    public List<BuildWarningEventArgs> BuildWarningEvents = new();
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore CA1002 // Do not expose generic lists

    public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
    {
      throw new NotImplementedException();
    }

    public void LogCustomEvent(CustomBuildEventArgs e)
    {
      CustomBuildEvents.Add(e);
    }

    public void LogErrorEvent(BuildErrorEventArgs e)
    {
      BuildErrorEvents.Add(e);
    }

    public void LogMessageEvent(BuildMessageEventArgs e)
    {
      BuildMessageEvents.Add(e);
    }

    public void LogWarningEvent(BuildWarningEventArgs e)
    {
      BuildWarningEvents.Add(e);
    }
  }
}
