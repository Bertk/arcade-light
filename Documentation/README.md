# **The original documentation is available [here](https://github.com/dotnet/arcade/tree/main/Documentation)**

Removed features:

- [arcade Versioning](https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Versioning.md) is replaced by [NerdbankGitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) and yaml templates
- use [Nuget Central Package Management](https://devblogs.microsoft.com/nuget/introducing-central-package-management/?WT.mc_id=DT-MVP-5004452) and remove nuget package versions from `Versions.props` file
- remove SourceBuild (feature)
- remove Helix channel support
- remove telemetry support (NETCORE_ENGINEERING_TELEMETRY)
- remove Maestro channel support
- remove Darc support
- remove micro build (microsoft internal)

=> add test code coverage using coverlet.msbuild
    - successful for local builds
    - issues with CI execution (see [coverlet known issues](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/KnownIssues.md))
