# Changelog DotNet.ArcadeLight.Sdk

All notable changes to this project will be documented in this file. See [keep a changelog](https://keepachangelog.com/en/1.1.0/) for format guidelines.
Use dotnet tool **nbgv** for release workflow (see [Preparing a release](https://github.com/dotnet/Nerdbank.GitVersioning/blob/v3.6.132/doc/nbgv-cli.md#preparing-a-release)).

## [Unreleased]

### Added

- added badges in ./README.md
- added GitHub actions
  - .github/workflows/dotnet.yml
  - .github/workflows/codeql.yml
  - .github/workflows/powershell.yml
- added .github/CODEOWNERS

## [1.4.20](https://github.com/Bertk/arcade-light/compare/v1.4.14...v1.4.20) - 2023-04-17

### Changed

- Updated .NET SDK to 7.0.203
- Updated dependencies
  - Microsoft.TemplateEngine.Tasks from 7.0.103 to 7.0.105
  - Microsoft.VSSDK.BuildTools from 17.5.4065 to 17.5.4072
- Updated .github/dependbot.yml

## [1.4.14](https://github.com/Bertk/arcade-light/compare/v1.3.17...v1.4.14) - 2023-03-29

### Changed

- activate CodeQL
- add workflow for .NET (manual build)
- rename package ID prefix (prepare upload to nuget.org)

## [1.3.17](https://github.com/Bertk/arcade-light/compare/v1.2.20...v1.3.17) - 2023-03-18

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

## [1.2.20](https://github.com/Bertk/arcade-light/compare/v1.1.15...v1.2.20) - 2023-02-22

### Added

- README.md for nuget package DotNet.ArcadeLight.Sdk
- DotNet.XUnitExtensions nuget package
- Unit tests LocateDotNetTests, SingleErrorTests, LocateDotNetVerify, CheckRequiredDotNetVersionTests, CompareVersionsTests

### Changed

- removed unused classes SetCorFlags, SaveItems, Unsign, ExponentialRetry, ZipArchiveManager

## [1.1.15](https://github.com/Bertk/arcade-light/compare/v1.0.11...v1.1.15) - 2023-01-26

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



## [1.0.11](https://github.com/Bertk/arcade-light/compare/v1.0.0...v1.0.11) - 2022-12-12

- initial release of DotNet.ArcadeLight.Sdk nuget package

### Added

- test code coverage using coverlet
  - successful for local builds only
  - automatically executes `dotnet publish` for test projects - required for code coverage generated executing vstest
  - issues with CI execution (see [coverlet known issues](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/KnownIssues.md))
