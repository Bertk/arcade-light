# Changelog DotNet.ArcadeLight.Sdk

## [Unreleased]

- n.a.

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
