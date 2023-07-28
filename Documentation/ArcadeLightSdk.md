# ArcadeLight SDK

ArcadeLight SDK is a reduced version of Microsoft .NET Arcade SDK. It is a set of msbuild props and targets files and packages that provide common build features used across multiple repos, such as CI integration, packaging, VSIX and VS setup authoring, testing.

ArcadeLight SDK does not use additional, proprietary Microsoft tools.

The goals are

- tiny build SDK for .NET projects
- to reduce the number of copies of the same or similar props, targets and script files across repos
- enable cross-platform build that relies on a standalone dotnet cli (downloaded during restore) as well as desktop msbuild based build
- be as close to the latest shipping .NET Core SDK as possible, with minimal overrides and tweaks

The toolset has four kinds of features and helpers:

- Common conventions applicable to all repos using the toolset.
- Infrastructure required for Azure DevOps CI builds.
- _Workarounds for bugs in shipping tools (dotnet SDK, VS SDK, msbuild, VS, NuGet client, etc.).
  Will be removed once the bugs are fixed in the product and the toolset moves to the new version of the tool._
- Abstraction of peculiarities of VSSDK and VS insertion process that are not compatible with dotnet SDK.

The toolset has following requirements on the repository layout.

## Single build output

All build outputs are located under a single directory called `artifacts`.
The ArcadeLight SDK defines the following output structure:

```text
artifacts
  bin
    $(MSBuildProjectName)
      $(Configuration)
  packages
    $(Configuration)
      Shipping
        $(MSBuildProjectName).$(PackageVersion).nupkg
      NonShipping
        $(MSBuildProjectName).$(PackageVersion).nupkg
      Release
      PreRelease
  TestResults
    $(Configuration)
      $(MSBuildProjectName)_$(TargetFramework)_$(TestArchitecture).(xml|html|log|error.log)
  VSSetup
    $(Configuration)
      Insertion
        $(VsixPackageId).json
        $(VsixPackageId).vsmand
        $(VsixContainerName).vsix
        $(VisualStudioInsertionComponent).vsman
      DevDivPackages
        $(MSBuildProjectName).$(PackageVersion).nupkg
      $(VsixPackageId).json
      $(VsixContainerName).vsix
  VSSetup.obj
    $(Configuration)
      $(VisualStudioInsertionComponent)
  SymStore
    $(Configuration)
      $(MSBuildProjectName)
  log
    $(Configuration)
      Build.binlog
  tmp
    $(Configuration)
  obj
    $(MSBuildProjectName)
      $(Configuration)
  toolset
```

Having a common output directory structure makes it possible to definitions.

| directory         | description |
|-------------------|-------------|
| bin               | Build output of each project. |
| obj               | Intermediate directory for each project. |
| packages          | NuGet packages produced by all projects in the repo. |
| SymStore          | Storage for converted Windows PDBs |
| log               | Build binary log and other logs. |
| tmp               | Temp files generated during build. |
| toolset           | Files generated during toolset restore. |
| TestResults       | Test result files. |

## Build scripts and extensibility points

### Build scripts

ArcadeLight provides common build scripts in the [eng/commonlight](https://github.com/Bertk/arcade-light/tree/main/eng/commonlight) folder:

- eng/common/[build.ps1](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/build.ps1)|[build.sh](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/build.sh)

  The scripts are designed to be used by repos that need a single `MSBuild` invocation to restore, build, package and test all projects in the repo. These scripts are thin wrappers calling into functions defined in `eng/common/tools.ps1|sh`. If the repository needs to run additional builds or commands it is recommended to create `eng/build.ps1|sh` scripts in the repository using `eng/common/build.ps1|sh` as a template and customize the implementation as necessary. These custom scripts should use common helpers and global variables defined in `eng/common/tools.ps1|sh` and provide command line switches that are a superset of the ones provided by `eng/common/build.ps1|sh`.

- eng/common/[tools.ps1](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/tools.ps1)|[tools.sh](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/tools.sh)

  Defines global variables and functions used in all builds scripts. This includes helpers that install .NET SDK, invoke MSBuild, locate Visual Studio, etc.

- eng/common/[CIBuild.cmd](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/CIBuild.cmd)|[cibuild.sh](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/cibuild.sh)

  Repositories that use `eng/common/build.ps1|sh` (as opposed to a customized `eng/build.ps1|sh`) should use this build script for the main build step in their pipeline definition. Repositories with custom `eng/build.ps1|sh` should also add the corresponding `eng/CIBuild.cmd|cibuild.sh` for use in their pipeline definition.

Repos may also provide a few convenience build scripts in the repository root that dispatch to either `eng/common/build.ps1|sh` or `eng/build.ps1|sh` (if repo uses customized build scripts) but do not implement any logic:

- [Build.cmd](https://github.com/Bertk/arcade-light/blob/main/Build.cmd) | [build.sh](https://github.com/Bertk/arcade-light/blob/main/build.sh) - default wrapper script for building and restoring the repo.
- [Restore.cmd](https://github.com/Bertk/arcade-light/blob/main/Restore.cmd) | [restore.sh](https://github.com/Bertk/arcade-light/blob/main/restore.sh) - default wrapper script for restoring the repo.
- [Test.cmd](https://github.com/Bertk/arcade-light/blob/main/Test.cmd) | [test.sh](https://github.com/Bertk/arcade-light/blob/main/test.sh) - default wrapper script for running tests in the repo.

Since the default scripts pass along additional arguments, you could restore, build, and test a repo by running `Build.cmd -test`.

You should feel free to create more repo specific scripts as appropriate to meet common dev scenarios for your repo.

### /eng/commonlight/*

The ArcadeLight SDK requires bootstrapper scripts to be present in the repo.

The scripts in this directory shall be present and the same across all repositories using ArcadeLight SDK.

### /Directory.Build.props

Provide repo specific Build properties and references for additional build tools.

`Directory.Build.props` shall import ArcadeLight SDK as shown below. The `Sdk.props` file sets various properties and item groups to default values. It is recommended to perform any customizations _after_ importing the SDK.

It is a common practice to specify properties applicable to all (most) projects in the repository in `Directory.Build.props`, e.g. public keys for `InternalsVisibleTo` project items.

```xml
<Project>
  <Import Project="Sdk.props" Sdk="DotNetDev.ArcadeLight.Sdk" />    
  <PropertyGroup> 
    <!-- Public keys used by InternalsVisibleTo project items -->
    <MoqPublicKey>00240000048000009400...</MoqPublicKey> 

    <!-- 
      Specify license used for packages produced by the repository.
      Use PackageLicenseExpressionInternal for closed-source licenses.
    -->
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    
  </PropertyGroup>
</Project>
```

### /Directory.Build.targets

`Directory.Build.targets` shall import ArcadeLight SDK. It may specify additional targets applicable to all source projects.

```xml
<Project>
  <Import Project="Sdk.targets" Sdk="DotNetDev.ArcadeLight.Sdk" />
</Project>
```

### /Directory.Packages.props

Provides single source of nuget package versions ( see [Central Package Management (CPM)](https://learn.microsoft.com/en-us/nuget/consume-packages/Central-Package-Management) )

#### ArcadeLight project building

By default, ArcadeLight builds solutions in the root of the repo.  Overriding the default build behavior may be done by either of these methods.

- Provide the project list on the command-line.

  Example: `build.cmd -projects MyProject.proj`

  See [source code](https://github.com/Bertk/arcade-light/blob/main/eng/commonlight/build.ps1#L63)

- Provide a list of projects or solutions in `Directory.Packages.props`.

  Example:

  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <!-- additional ItemGroup in Directory.Packages.props -->
  <Project>
    <ItemGroup>
      <ProjectToBuild Include="$(MSBuildThisFileDirectory)..\MyProject.proj" />
    </ItemGroup>
  </Project>
  ```

Note: listing both project files formats (such as .csproj) and solution files (.sln) at the same time is not currently supported.

#### Example: specifying a solution to build

This is often required for repos which have multiple .sln files in the root directory.

```xml
<!-- additional ItemGroup in Directory.Packages.props -->
<Project>
  <ItemGroup>
    <ProjectToBuild Include="$(RepoRoot)MySolution1.sln" />
  </ItemGroup>
</Project>
```

#### Example: building project files instead of solutions

You can also specify a list of projects to build instead of building .sln files.

```xml
<!-- additional ItemGroup in Directory.Packages.props -->
<Project>
  <ItemGroup>
    <ProjectToBuild Include="$(RepoRoot)src\**\*.csproj" />
  </ItemGroup>
</Project>
```

#### Example: conditionally specifying which projects to build

You can use custom MSBuild properties to control the list of projects which build.

```xml
<!-- additional ItemGroup in Directory.Packages.props -->
<Project>
  <ItemGroup>
    <!-- Usage: build.cmd /p:BuildMyOptionalGroupOfStuff=true -->
    <ProjectToBuild Condition="'$(BuildMyOptionalGroupOfStuff)' == 'true"
                    Include="$(RepoRoot)src\feature1\**\*.csproj" />

    <!-- Only build some projects when building on Windows  -->
    <ProjectToBuild Condition="'$(OS)' == 'Windows_NT"
                    Include="$(RepoRoot)src\**\*.vcxproj" />

    <!-- You can also use MSBuild Include/Exclude syntax -->
    <ProjectToBuild Include="$(RepoRoot)src\**\*.csproj"
                    Exclude="$(RepoRoot)src\samples\**\*.csproj" />
  </ItemGroup>
</Project>
```

#### Example: custom implementations of 'Restore'

By default, ArcadeLight assumes that the 'Restore' target on projects is implemented using NuGet's restore. If that is not the case, you can opt-out of some ArcadeLight
optimizations by setting 'RestoreUsingNuGetTargets' to false.

```xml
<!-- additional ItemGroup in Directory.Packages.props -->
<Project>
  <PropertyGroup>
    <RestoreUsingNuGetTargets>false</RestoreUsingNuGetTargets>
  </PropertyGroup>
  <ItemGroup>
    <ProjectToBuild Include="$(RepoRoot)src\dir.proj" />
  </ItemGroup>
</Project>
```

The toolset also defines default versions for various dependencies, such as XUnit, VSSDK, etc. These defaults can be overridden in the Versions.props file.

See [DefaultVersions (legacy)](https://github.com/Bertk/arcade-light/blob/main/src/DotNetDev.ArcadeLight.Sdk/tools/DefaultVersions.props) for a list of _UsingTool_ properties and default versions.

### /global.json

`/global.json` file is present and specifies the version of the dotnet and `DotNetDev.ArcadeLight.Sdk` SDKs.

For example,

```json
{
  "tools": {
    "dotnet": "6.0.402"
  },
  "msbuild-sdks": {
    "DotNetDev.ArcadeLight.Sdk": "1.4.14"
  }
}
```

Note: defining `runtimes` in your global.json will signal to ArcadeLight to install a local version of the SDK for the runtimes to use rather than depending on a matching global SDK.

### /NuGet.config

`/NuGet.config` file is present and specifies the MyGet feed to retrieve ArcadeLight SDK from and other feeds required by the repository like so:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <!-- Feed to use to restore the ArcadeLight SDK from -->  
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <!-- Feeds to use to restore dependent packages from -->  
    <add key="my-feed" value="https://dotnet.myget.org/F/myfeed/api/v3/index.json" />
  </packageSources>
  <disabledPackageSources>
    <clear />
  </disabledPackageSources>
</configuration>
```

### /License.txt

The root of the repository shall include a license file named `license.txt`, `license.md` or `license` (any casing is allowed).
It is expected that all packages built from the repository have the same license, which is the license declared in the repository root license file.

If the repository uses open source license it shall specify the license name globally using `PackageLicenseExpression` property, e.g. in [Directory.Build.props](https://github.com/Bertk/arcade-light/blob/main/Directory.Build.props).

This validation can be suppressed by setting `SuppressLicenseValidation` to `true` if necessary (not recommended).

See [NuGet documentation](https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets#packing-a-license-expression-or-a-license-file) for details.

### Source Projects

Projects are located under `src` directory under root repo, in any subdirectory structure appropriate for the repo.

Projects shall use `Microsoft.NET.Sdk` SDK like so:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    ...
</Project>
```

### Project name conventions

- Unit test project file names shall end with `.UnitTests` or `.Tests`, e.g. `MyProject.UnitTests.csproj` or `MyProject.Tests.csproj`.
- Integration test project file names shall end with `.IntegrationTests`, e.g. `MyProject.IntegrationTests.csproj`.
- Performance test project file names shall end with `.PerformanceTests`, e.g. `MyProject.PerformanceTests.csproj`.
- If `source.extension.vsixmanifest` is present next to the project file the project is by default considered to be a VSIX producing project.

## Other Projects

It might be useful to create other top-level directories containing projects that are not standard C#/VB/F# projects. For example, projects that aggregate outputs of multiple projects into a single NuGet package or Willow component. These projects should also be included in the main solution so that the build driver includes them in build process, but their `Directory.Build.*` may be different from source projects. Hence the different root directory.

## Building source packages

ArcadeLight SDK provides targets for building source packages.

Set `IsSourcePackage` to `true` to indicate that the project produces a source package (along with `IsPackable`, `PackageDescription` and other package properties).

If the project does not have an explicitly provided `.nuspec` file (`NuspecFile` property is empty) setting `IsSourcePackage` to `true` will trigger a target that
puts sources contained in the project directory to the `contentFiles` directory of the source package produced by the project.

In addition a `build/$(PackageId).targets` file will be auto-generated that links the sources contained in the package to the source server via a Source Link target.
If your package already has a `build/$(PackageId).targets` file set `SourcePackageSourceLinkTargetsFileName` property to a different file name (e.g. `SourceLink.targets`)
and import the file from `build/$(PackageId).targets`.

If the project is packaged using a custom `.nuspec` file then the source and targets files must be listed in the `.nuspec` file. The path to the generated Source Link
targets file will be available within the `.nuspec` file via variable `$SourceLinkTargetsFilePath$`.

## Common steps in Azure DevOps pipeline

The steps below assume the following variables to be defined:

- `BuildConfiguration` - the build configuration "Debug" or "Release"

### Official build script

### Publishing test results

```yml
- task: PublishTestResults@2
  displayName: Publish Test Results
  inputs:
    testRunner: XUnit
    testResultsFiles: 'artifacts/$(BuildConfiguration)/TestResults/*.trx'
    mergeTestResults: true
    testRunTitle: 'Unit Tests'
  condition: always()
```

### Publishing logs

:warning: `PublishBuildArtifacts@1` is deprecated use instead [publish](https://learn.microsoft.com/en-us/azure/devops/pipelines/artifacts/pipeline-artifacts?view=azure-devops&tabs=yaml)

```yml
- task: PublishBuildArtifacts@1
    displayName: Publish Logs
    inputs:
      PathtoPublish: '$(Build.SourcesDirectory)\artifacts\log\$(BuildConfiguration)'
      ArtifactName: '$(OperatingSystemName) $(BuildConfiguration)'
    continueOnError: true
    condition: not(succeeded())
```

## Project Properties Defined by the SDK

### `IsShipping`, `IsShippingAssembly`, `IsShippingPackage`, `IsShippingVsix` (bool)

`IsShipping-` properties are project properties that determine which (if any) assets produced by the project are _shipping_. An asset is considered _shipping_ if it is intended to be delivered to customers via an official channel. This channel can be NuGet.org, an official installer, etc. Setting this flag to `true` does not guarantee that the asset will actually ship in the next release of the product. It might be decided after the build is complete that although the artifact is ready for shipping it won't be shipped this release cycle.

By default all assets produced by a project are considered _shipping_. Set `IsShipping` to `false` if none of the assets produced by the project are _shipping_. Test projects (`IsTestProject` is `true`) set `IsShipping` to `false` automatically.

Setting `IsShipping` property is sufficient for most projects. Projects that produce both _shipping_ and _non-shipping_ assets need a finer grained control. Set `IsShippingAssembly`, `IsShippingPackage` or `IsShippingVsix` to `false` if the assembly, package, or VSIX produced by the project is not _shipping_, respectively.

Build targets shall not directly use `IsShipping`. Instead they shall use `IsShippingAssembly`, `IsShippingPackage` and `IsShippingVsix` depending on the asset they are dealing with.

Examples of usage:

- Set `IsShipping` property to `false` in test/build/automation utility projects.

- Set `IsShipping` property to `false` in projects that produce VSIX packages that are only used only within the repository (e.g. to facilitate integration tests or VS F5) and not expected to be installed by customers.

- Set `IsShippingPackage` property to `false` in projects that package  _shipping_ assemblies in packages that facilitate transport of assets from one repository to another one, which extracts the assemblies and _ships_ them in a  _shipping_ container.

All assemblies, packages and VSIXes are signed by default, regardless of whether they are _shipping_ or not.

By default, Portable and Embedded PDBs produced by _shipping_ projects are converted to Windows PDBs and published to Microsoft symbol servers.

By default, all _shipping_ libraries are localized.

### `IsVisualStudioBuildPackage` (bool)

Set to `true` in projects that build Visual Studio Build packages. These packages are non-shipping, but their content is shipping. They are inserted into and referenced from the internal DevDiv `VS` repository.

### `SkipTests` (bool)

Set to `true` in a test project to skip running tests.

### `TestArchitectures` (list of strings)

List of test architectures (`x64`, `x86`) to run tests on.
If not specified by the project defaults to the value of `PlatformTarget` property, or `x64` if `Platform` is `AnyCPU` or unspecified.

For example, a project that targets `AnyCPU` can opt-into running tests using both 32-bit and 64-bit test runners on .NET Framework by setting `TestArchitectures` to `x64;x86`.

### `TestTargetFrameworks` (list of strings)

By default, the test runner will run tests for all frameworks a test project targets. Use `TestTargetFrameworks` to reduce the set of frameworks to run against.

For example, consider a project that has `<TargetFrameworks>net6.0</TargetFrameworks>`. To only run .NET Core tests run

```text
msbuild Project.UnitTests.csproj /t:Test /p:TestTargetFrameworks=net6.0
```

To specify multiple target frameworks on command line quote the property value like so:

```text
msbuild Project.UnitTests.csproj /t:Test /p:TestTargetFrameworks="net6.0;net472"
```

### `TestRuntime` (string)

Runtime to use for running tests. Currently supported values are: `Core` (.NET Core), `Full` (.NET Framework).

### `TestTimeout` (int)

Timeout to apply to an individual invocation of the test runner for a single configuration. Integer number of milliseconds.

### `GenerateResxSource` (bool)

When set to true, ArcadeLight will generate a class source for all embedded .resx files.

If source should only be generated for some .resx files, this can be turned on for individual files like this:

```xml
<ItemGroup>
   <EmbeddedResource Update="MyResources.resx" GenerateSource="true" />
</ItemGroup>
```

The contents of the generated source can be fine-tuned with these additional settings.

#### `GenerateResxSourceEmitFormatMethods` (bool)

When a string in the resx file has argument placeholders, generate a `.FormatXYZ(...)` method with parameters for each placeholder in the string.

Example: if the resx file contains a string "This has {0} and {1} placeholders", this method will be generated:

```c#
class Resources
{
  // ...
  public static string FormatMyString(object p0, object p1) { /* ..uses string.Format()... */ }
}
```

#### `GenerateResxSourceIncludeDefaultValues` (bool)

If set to true calls to GetResourceString receive a default resource string value.

#### `GenerateResxSourceOmitGetResourceString` (bool)

If set to true the GetResourceString method is not included in the generated class and must be specified in a separate source file.
