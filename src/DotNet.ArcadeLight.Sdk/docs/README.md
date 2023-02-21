## About

Lightweight package of dotnet Arcade (does not use Microsoft proprietary tooling)

## How to Use

### Configuration

#### 1) add global.json

```json
{
  "tools": {
    "dotnet": "7.0.103"
  },
  "msbuild-sdks": {
    "DotNet.ArcadeLight.Sdk": "1.1.16"
  }
}
```

#### 2) Add line `<Import Project="Sdk.props" ...\>` in Directory.Build.props

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.props" Sdk="Microsoft.DotNet.Arcade.Sdk" />
  ...
<\Project>
```

#### 3) Add line `<Import Project="Sdk.targets" ...\>` in Directory.build.targets

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <Import Project="Sdk.targets" Sdk="Microsoft.DotNet.Arcade.Sdk" />
  ...
<\Project>
```

#### 4) Copy `eng\commonlight` from Arcade-light into repo.

#### 5) Add the Versions.props file to your eng\ folder

#### 6) copy `version.json` to repository root folder

#### 7) create `nuget.config` file with a source for the DotNet.ArcadeLight.Sdk nuget package

#### 8) optionally copy the scripts for `restore`, `build` and `test` to repository root folder

### Use ArcadeLight with command shell or Visual Studio

```
> build
> test
```

## Related documentation

* https://github.com/Bertk/arcade-light/blob/main/Documentation/ArcadeLightSdk.md
* https://github.com/dotnet/arcade/tree/main/Documentation

## Related Packages

* https://github.com/dotnet/arcade
* https://github.com/coverlet-coverage/coverlet
