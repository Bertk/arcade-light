# Changelog DotNet.ArcadeLight.Sdk

All notable changes to this project will be documented in this file.
See [keep a changelog](https://keepachangelog.com/en/1.1.0/) for format guidelines.

Use dotnet tool **nbgv** for release workflow (see [Preparing a release](https://github.com/dotnet/Nerdbank.GitVersioning/blob/v3.6.132/doc/nbgv-cli.md#preparing-a-release)).

## [Unreleased]

### Changed

- Bump Microsoft.NET.StringTools from 17.6.3 to 17.7.0 (#189)
- Bump Microsoft.Build.Framework from 17.6.3 to 17.7.0 (#188)
- Bump Microsoft.Build from 17.6.3 to 17.7.0 (#187)
- Bump NuGet.Versioning from 6.6.1 to 6.7.0 (#186)
- Bump Moq from 4.20.1 to 4.20.2 (#185)
- Bump Microsoft.Build.Utilities.Core from 17.6.3 to 17.7.0 #184
- Bump Microsoft.Build.Tasks.Core from 17.6.3 to 17.7.0 (#183)
- Bump NuGet.Frameworks from 6.6.1 to 6.7.0 (#182)
- update .NET SDK 7.0.400 (#181)
- Bump SonarAnalyzer.CSharp from 9.6.0.74858 to 9.7.0.75501 (#180)
- Bump Microsoft.TestPlatform.TestHost from 17.6.3 to 17.7.0 (#179)
- Bump Microsoft.TestPlatform.ObjectModel from 17.6.3 to 17.7.0 (#178)
- Bump Microsoft.NET.Test.Sdk from 17.6.3 to 17.7.0 (#177)
- Bump Microsoft.TemplateEngine.Tasks from 7.0.109 to 7.0.110 (#176)
- Bump Moq from 4.18.4 to 4.20.1 (#175)
- Bump SonarAnalyzer.CSharp from 9.5.0.73987 to 9.6.0.74858 (#174)
- Bump danielpalme/ReportGenerator-GitHub-Action from 5.1.22 to 5.1.23 (#173)


## [1.6.3](https://github.com/Bertk/arcade-light/compare/v1.6.2...v1.6.3) - 2023-07-15

### Changed

- Fix nuget package content (#167)

## [1.6.2](https://github.com/Bertk/arcade-light/compare/v1.5.81...v1.6.2) - 2023-07-14

### Changed

- Update DefaultVersions.props (coverlet.collector, xunit, xunit.runner.visualstudio) (#164)
- Remove useless targets (#156)
- Improve CheckRequiredDotNetVersionTests (#155)
- Update dependencies
  - Bump Microsoft.NETCore.Platforms from 7.0.3 to 7.0.4 (#161)
  - Bump Microsoft.TemplateEngine.Tasks from 7.0.108 to 7.0.109 (#158)
  - Bump cyclonedx from 2.8.0 to 2.8.1 (#160)

## [1.5.81](https://github.com/Bertk/arcade-light/compare/v1.5.53...v1.5.81) - 2023-07-07

### Changed

- use $(NetMinimum) and $(NetCurrent) for SDK (prepare net8.0 updates)
- Fix SBOM (ignore tests)
- fix warnings (#145)
- Updated dependencies
  - Bump xunit from 2.4.2 to 2.5.0 (#151)
  - Bump xunit.extensibility.execution from 2.4.2 to 2.5.0 (#148)
  - Bump xunit.runner.console from 2.4.2 to 2.5.0 (#149)
  - Bump xunit.extensibility.core from 2.4.2 to 2.5.0 (#150)
  - Bump dotnet-reportgenerator-globaltool from 5.1.22 to 5.1.23 (#152)
  - Bump vswhere from 3.1.4 to 3.1.7 (#147)
  - Bump xunit.runner.visualstudio from 2.4.5 to 2.5.0 (#146)
  - Bump cyclonedx from 2.7.0 to 2.8.0 (#143)
  - Bump danielpalme/ReportGenerator-GitHub-Action from 5.1.21 to 5.1.22 (#142)
  - Bump Microsoft.TemplateEngine.Tasks from 7.0.107 to 7.0.108 (#144)
  - Update Microsoft.NET.Test.Sdk version (#141)
  - Bump NuGet.Frameworks from 6.6.0 to 6.6.1 (#139)
  - Bump Microsoft.NETCore.Platforms from 7.0.2 to 7.0.3 (#136)
  - Bump System.Reflection.Metadata from 7.0.1 to 7.0.2 (#137)
  - Bump NuGet.Versioning from 6.6.0 to 6.6.1 (#135)
  - Bump System.Text.Json from 7.0.2 to 7.0.3 (#138)

## [1.5.53](https://github.com/Bertk/arcade-light/compare/v1.5.35...v1.5.53) - 2023-06-05

### Changed

- Updated dependencies
  - Bump Nerdbank.GitVersioning from 3.6.132 to 3.6.133
  - Bump coverlet.collector from 3.2.0 to 6.0.0
  - Bump danielpalme/ReportGenerator-GitHub-Action from 5.1.20 to 5.1.21
  - Bump Bump dotnet-reportgenerator-globaltool from 5.1.20 to 5.1.21
  - Microsoft.Build.Framework from 17.5.0 to 17.6.3
  - Microsoft.Build.Tasks.Core from 17.5.0 to 17.6.3
  - Microsoft.Build.Utilities.Core from 17.5.0 to 17.6.3
  - Bump Microsoft.NET.Test.Sdk from 17.6.0 to 17.6.1
  - Bump Microsoft.TestPlatform from 17.6.0 to 17.6.1
  - Bump Microsoft.TestPlatform.ObjectModel from 17.6.0 to 17.6.1
  - Bump Microsoft.TestPlatform.TestHost from 17.6.0 to 17.6.1
  - Bump Microsoft.VSSDK.BuildTools from 17.5.4074 to 17.6.2164
  - Bump nbgv from 3.6.132 to 3.6.133
  - Bump NuGet.Versioning from 6.5.0 to 6.6.0
  - Bump NuGet.Frameworks from 6.5.0 to 6.6.0
  - Bump NUnit3TestAdapter from 4.4.2 to 4.5.0

## [1.5.35](https://github.com/Bertk/arcade-light/compare/v1.4.20...v1.5.35) - 2023-05-17

### Added

- added badges in ./README.md
- added GitHub actions
  - .github/workflows/dotnet.yml
  - .github/workflows/codeql.yml
  - .github/workflows/powershell.yml
- added .github/CODEOWNERS
- added .markdownlint.json

### Changed

- Update dotnet SDK and nuget versions
- Avoid deprecated nuget packages
- Updated dependencies
  - Bump Nerdbank.GitVersioning from 3.6.128 to 3.6.132
  - Bump nbgv from 3.6.128 to 3.6.132
  - Bump danielpalme/ReportGenerator-GitHub-Action from 5.1.19 to 5.1.20
  - Bump dotnet-reportgenerator-globaltool from 5.1.19 to 5.1.20
  - Bump SonarAnalyzer.CSharp from 8.55.0.65544 to 9.0.0.68202

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
