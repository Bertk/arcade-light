# **The original documentation is available [here](https://github.com/dotnet/arcade/tree/main/Documentation)**

##Removed features:

- [arcade Versioning](https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Versioning.md) is replaced by [NerdbankGitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) and yaml templates
- use [Nuget Central Package Management](https://devblogs.microsoft.com/nuget/introducing-central-package-management/?WT.mc_id=DT-MVP-5004452) and remove nuget package versions from `Versions.props` file
- removed SourceBuild (feature)
- removed Helix channel support
- removed telemetry support (NETCORE_ENGINEERING_TELEMETRY)
- removed Maestro channel support
- removed Darc support
- removed micro build (microsoft internal)
- removed sign.proj and pack.proj (should be handled in build pipeline)
- removed PublishToSymbolServer 
- removed SetCorFlags
- removed GenerateChecksums
- removed ExtractNgenMethodList

## new features

- add test code coverage using coverlet
    - successful for local and CI builds
    - issues with CI execution (see [coverlet known issues](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/KnownIssues.md))
