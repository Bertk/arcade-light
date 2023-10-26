rem dotnet tool update dotnet-reportgenerator-globaltool -g
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"artifacts\reports" -reporttypes:"Html;HtmlInline_AzurePipelines_Dark;Cobertura" -assemblyfilters:"-xunit;-DotNet.XUnitExtensions;-DotNetDev.ArcadeLight.Test.Common;-DotNet.Internal.DependencyInjection.Testing"
