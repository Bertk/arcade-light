# Changelog DotNet.ArcadeLight.Sdk

## [Unreleased]

### Added

- `PackageReleaseNotes` property for nuget package release notes `CHANGELOG.md`

### Changed

- TODO
  - coverlet coverage should use published artifacts for test

## [1.0.11] - 2022-12-12

- initial release of DotNet.ArcadeLight.Sdk nuget package

### Added

- test code coverage using coverlet
  - successful for local builds only
  - automatically executes `dotnet publish` for test projects - required for code coverage generated executing vstest
  - issues with CI execution (see [coverlet known issues](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/KnownIssues.md))
