using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notie;
using Tasky.Core.Infrastructure;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Initialization;

public static class Register
{
    public static void RegisterTaskyCore(this IServiceCollection services)
    {
        services.RegisterHandlers();
        services.RegisterInfrastructure();
        services.AddNotie();
    }

    private static void RegisterHandlers(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    private static void RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(_ => GetConfiguration());
        services.AddScoped<IContext, FileDbContext>();
        services.AddScoped<IBoardRepository, BoardRepository>();
    }

    private static Configuration GetConfiguration()
    {
        var basePath = OperatingSystem.IsWindows()
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasky")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "tasky");

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(Configuration.SettingsFilename, optional: false, reloadOnChange: true)
            .AddJsonFile("tasky.override.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("TASKY_");

#if DEBUG
        configurationBuilder.AddJsonFile(Configuration.DevelopmentSettingsFilename, optional: true, reloadOnChange: true);
#endif

        var config = configurationBuilder.Build();

        var useLocalFile = Convert.ToBoolean(config[Configuration.UseLocalFileProperty] ?? "true");
        var overridePath = config["DatabasePathOverride"];

        if (!string.IsNullOrEmpty(overridePath))
        {
            return new Configuration
            {
                UseLocalFile = useLocalFile,
                DatabasePath = Path.Combine(overridePath, Configuration.DatabaseFilename)
            };
        }

        return new Configuration
        {
            UseLocalFile = useLocalFile,
            DatabasePath = Path.Combine(basePath, Configuration.DatabaseFilename)
        };
    }

}