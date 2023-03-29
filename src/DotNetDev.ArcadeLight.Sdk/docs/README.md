## About

Lightweight package of [dotnet Arcade](https://github.com/dotnet/arcade) without Microsoft proprietary tooling

## How to Use

### Configuration

#### 1) add global.json

```json
{
  "tools": {
    "dotnet": "7.0.202"
  },
  "msbuild-sdks": {
    "DotNetDev.ArcadeLight.Sdk": "1.4.12"
  }
}
```

#### 2) create `nuget.config` file with a source for the *DotNetDev.ArcadeLight.Sdk* nuget package

#### 3) Add lines in Directory.Build.props

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.props" Sdk="DotNetDev.ArcadeLight.Sdk" />

  <PropertyGroup>
      <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  ...
<\Project>
```

#### 4) Add line in Directory.build.targets

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.targets" Sdk="DotNetDev.ArcadeLight.Sdk" />
  ...
<\Project>
```

#### 5) Add lines in Directory.Packages.props

```xml
<Project>
  <PropertyGroup>
    <CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
  </PropertyGroup>

  <ItemGroup>
    <GlobalPackageReference Include="Nerdbank.GitVersioning" Version="3.5.119" />
  </ItemGroup>
  ...
<\Project>
```

#### 6) Copy `eng\commonlight` from Arcade-light into repo.

#### 7) Add the Versions.props file to your eng\ folder

#### 8) copy `version.json` to repository root folder

#### 9) optionally copy the scripts for `restore`, `build` and `test` to repository root folder

### Use ArcadeLight with command shell or Visual Studio

```shell
build
test
```

## Related documentation

* https://github.com/Bertk/arcade-light/blob/main/Documentation/ArcadeLightSdk.md
* https://github.com/dotnet/arcade/tree/main/Documentation

## Related Packages

* https://github.com/dotnet/arcade
* https://github.com/coverlet-coverage/coverlet
