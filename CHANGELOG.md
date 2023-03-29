# Changelog DotNet.ArcadeLight.Sdk

## [Unreleased]

- n.a.

## [1.4.14] - 2023-03-29

### Changed

- activate CodeQL
- add workflow for .NET (manual build)
- rename package ID prefix (prepare upload to nuget.org)

## [1.3.17] - 2023-03-18

### Changed

- update nuget package tags
- update dependencies
  - dotnet-reportgenerator-globaltool from 5.1.18 to 5.1.19
  - SonarAnalyzer.CSharp from 8.53.0.62665 to 8.54.0.64047
  - Microsoft.Build from 17.4.0 to 17.5.0
  - Microsoft.Build.Framework from 17.4.0 to 17.5.0
  - Microsoft.Build.Tasks.Core from 17.4.0 to 17.5.0
  - Microsoft.Build.Utilities.Core from 17.4.0 to 17.5.0
  - Microsoft.NET.StringTools from 17.4.0 to 17.5.0
  - Newtonsoft.Json from 13.0.2 to 13.0.3
  - NuGet.Versioning from 6.4.0 to 6.5.0
  - NuGet.Frameworks from 6.4.0 to 6.5.0
  - NUnit3TestAdapter from 4.3.1 to 4.4.2
  - System.Reflection.Metadata from 7.0.0 to 7.0.1

## [1.2.20] - 2023-02-22

### Added

- README.md for nuget package DotNet.ArcadeLight.Sdk
- DotNet.XUnitExtensions nuget package
- Unit tests LocateDotNetTests, SingleErrorTests, LocateDotNetVerify, CheckRequiredDotNetVersionTests, CompareVersionsTests

### Changed

- removed unused classes SetCorFlags, SaveItems, Unsign, ExponentialRetry, ZipArchiveManager

## [1.1.15] - 2023-01-26

### Added

- Documentation
  - CHANGELOG.md
  - binary log viewer information (hints only)

### Changed



- `PackageReleaseNotes` property for nuget package release notes `CHANGELOG.md`
- update dependencies
  - Microsoft.TemplateEngine.Tasks from 7.0.100 to 7.0.102
  - vswhere from 3.0.3 to 3.1.1
  - System.Security.Cryptography.Xml from 7.0.0 to 7.0.1
  - NUnit3TestAdapter from 4.2.1 to 4.3.1



## [1.0.11] - 2022-12-12

- initial release of DotNet.ArcadeLight.Sdk nuget package

### Added

- test code coverage using coverlet
  - successful for local builds only
  - automatically executes `dotnet publish` for test projects - required for code coverage generated executing vstest
  - issues with CI execution (see [coverlet known issues](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/KnownIssues.md))
