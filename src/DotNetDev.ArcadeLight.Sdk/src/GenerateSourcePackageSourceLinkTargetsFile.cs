// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;

namespace DotNetDev.ArcadeLight.Sdk
{
  public sealed class GenerateSourcePackageSourceLinkTargetsFile : Microsoft.Build.Utilities.Task
  {
    [Required]
    public string ProjectDirectory { get; set; }

    [Required]
    public string PackageId { get; set; }

    [Required]
#pragma warning disable CA1819 // Properties should not return arrays
    public ITaskItem[] SourceRoots { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    [Required]
    public string OutputPath { get; set; }

    public override bool Execute()
    {
      ExecuteImpl();
      return !Log.HasLoggedErrors;
    }

    private void ExecuteImpl()
    {
      Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
      File.WriteAllText(OutputPath, GetOutputFileContent(), Encoding.UTF8);
    }

    // for testing
    internal string GetOutputFileContent()
    {
      string projectDir = EndWithSeparator(ProjectDirectory);

      string innerMostRootItemSpec = null;
      string innerMostRootSourceLinkUrl = null;
      foreach (ITaskItem sourceRoot in SourceRoots)
      {
        string itemSpec = sourceRoot.ItemSpec;
        if (itemSpec.Length > projectDir.Length && itemSpec.StartsWith(projectDir, StringComparison.Ordinal))
        {
          Log.LogError($"Directory '{ProjectDirectory}' contains source roots (e.g. git submodules), which is not supported.");
          return null;
        }

        if (!projectDir.StartsWith(itemSpec, StringComparison.Ordinal) ||
            innerMostRootItemSpec != null && itemSpec.Length <= innerMostRootItemSpec.Length)
        {
          continue;
        }

        string url = sourceRoot.GetMetadata("SourceLinkUrl");
        if (string.IsNullOrEmpty(url))
        {
          continue;
        }

        innerMostRootItemSpec = itemSpec;
        innerMostRootSourceLinkUrl = url;
      }

      if (innerMostRootSourceLinkUrl == null)
      {
        Log.LogError($"No SourceRoot with SourceLinkUrl contains directory '{ProjectDirectory}'.");
        return null;
      }

      if (innerMostRootSourceLinkUrl.Count(c => c == '*') != 1)
      {
        Log.LogError($"SourceLinkUrl must contain exactly one '*': '{innerMostRootSourceLinkUrl}'");
        return null;
      }

      if (!EndsWithSeparator(innerMostRootItemSpec))
      {
        Log.LogError($"SourceRoot must end with a directory separator: '{innerMostRootItemSpec}'");
        return null;
      }

      string relativePathToSourceRoot = projectDir.Substring(innerMostRootItemSpec.Length);
      string contentFilesSourceLinkUrl = innerMostRootSourceLinkUrl.Replace("*", Uri.EscapeUriString(relativePathToSourceRoot.Replace('\\', '/')) + "*");
      return GetTargetsFileContent(PackageId, contentFilesSourceLinkUrl);
    }

    private static bool EndsWithSeparator(string path)
    {
      char last = path[path.Length - 1];
      return last == Path.DirectorySeparatorChar || last == Path.AltDirectorySeparatorChar;
    }

    private static string EndWithSeparator(string path)
        => EndsWithSeparator(path) ? path : path + Path.DirectorySeparatorChar;

    private static string GetTargetsFileContent(string packageId, string sourceLinkUrl)
    {
      string hash;
#pragma warning disable CA1850
      using (var hashAlg = SHA256.Create())
      {
        hash = BitConverter.ToString(hashAlg.ComputeHash(Encoding.UTF8.GetBytes(packageId)), 0, 20).Replace("-", "");
      }
#pragma warning restore CA1850
      return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Target Name=""_AddSourcePackageSourceRoot_{hash}"" BeforeTargets=""InitializeSourceControlInformation"">
    <ItemGroup>
      <_PackageCompileItems Remove=""@(_PackageCompileItems)""/>
      <_PackageCompileItems Include=""%(Compile.RootDir)%(Compile.Directory)"" Condition=""'%(Compile.NuGetPackageId)' == '{packageId}'"" />
    </ItemGroup>

    <PropertyGroup>
      <_PackageCompileItem>@(_PackageCompileItems)</_PackageCompileItem>
      <_PackageCompileItem Condition=""'$(_PackageCompileItem)' != ''"">$(_PackageCompileItem.Split(';')[0])</_PackageCompileItem>
    </PropertyGroup>

    <ItemGroup>
      <SourceRoot Include=""$([System.Text.RegularExpressions.Regex]::Match($(_PackageCompileItem), '^$([System.Text.RegularExpressions.Regex]::Escape($([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)../contentFiles/'))))([^\\/]+[\\/][^\\/]+[\\/])'))"" 
                  SourceLinkUrl=""{sourceLinkUrl}""/>
    </ItemGroup>
  </Target>
</Project>
";
    }
  }
}
