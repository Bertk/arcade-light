// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

using System;
using System.Reflection;

namespace Xunit
{
  public static class CoreClrConfigurationDetection
  {
    public static bool IsJitStress => !string.Equals(GetEnvironmentVariableValue("JitStress"), "0", StringComparison.Ordinal);
    public static bool IsJitStressRegs => !string.Equals(GetEnvironmentVariableValue("JitStressRegs"), "0", StringComparison.Ordinal);
    public static bool IsJitMinOpts => string.Equals(GetEnvironmentVariableValue("JITMinOpts"), "1", StringComparison.Ordinal);
    public static bool IsTailCallStress => string.Equals(GetEnvironmentVariableValue("TailcallStress"), "1", StringComparison.Ordinal);
    public static bool IsZapDisable => string.Equals(GetEnvironmentVariableValue("ZapDisable"), "1", StringComparison.Ordinal);
    public static bool IsGCStress3 => CompareGCStressModeAsLower(GetEnvironmentVariableValue("GCStress"), "0x3", "3");
    public static bool IsGCStressC => CompareGCStressModeAsLower(GetEnvironmentVariableValue("GCStress"), "0xC", "C");

    public static bool IsGCStress => !string.Equals(GetEnvironmentVariableValue("GCStress"), "0", StringComparison.Ordinal);

    public static bool IsCheckedRuntime => AssemblyConfigurationEquals("Checked");
    public static bool IsReleaseRuntime => AssemblyConfigurationEquals("Release");
    public static bool IsDebugRuntime => AssemblyConfigurationEquals("Debug");

#pragma warning disable S1067 // Expressions should not be too complex
    public static bool IsStressTest =>
            IsGCStress ||
            IsZapDisable ||
            IsTailCallStress ||
            IsJitStressRegs ||
            IsJitStress ||
            IsJitMinOpts;
#pragma warning restore S1067 // Expressions should not be too complex

    private static string GetEnvironmentVariableValue(string name) =>
            Environment.GetEnvironmentVariable("DOTNET_" + name) ?? Environment.GetEnvironmentVariable("COMPlus_" + name) ?? "0";

    private static bool AssemblyConfigurationEquals(string configuration)
    {
      AssemblyConfigurationAttribute assemblyConfigurationAttribute = typeof(string).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();

      return assemblyConfigurationAttribute != null &&
          string.Equals(assemblyConfigurationAttribute.Configuration, configuration, StringComparison.Ordinal);
    }

    private static bool CompareGCStressModeAsLower(string value, string first, string second)
    {
      value = value.ToLowerInvariant();
      return string.Equals(value, first.ToLowerInvariant(), StringComparison.Ordinal) ||
          string.Equals(value, second.ToLowerInvariant(), StringComparison.Ordinal) ||
          string.Equals(value, "0xf", StringComparison.Ordinal) ||
          string.Equals(value, "f", StringComparison.Ordinal);
    }
  }
}
