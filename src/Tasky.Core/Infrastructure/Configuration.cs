namespace Tasky.Core.Infrastructure;

public class Configuration
{
    public const string DevelopmentSettingsFilename = "appsettings.dev.json";
    public const string SettingsFilename = "appsettings.json";
    public const string DatabaseFilename = "database.json";
    public const string UseLocalFileProperty = "UseLocalFile";
    public const string DatabasePathOverrideProperty = "DatabasePathOverride";

    public bool UseLocalFile { get; init; }
    public string DatabasePath { get; init; } = "";
}
