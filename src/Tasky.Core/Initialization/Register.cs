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
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile(Configuration.SettingsFilename);

#if DEBUG
        configurationBuilder.AddJsonFile(Configuration.DevelopmentSettingsFilename);
#endif

        var config = configurationBuilder.Build();

        var useLocalFile = Convert.ToBoolean(config[Configuration.UseLocalFileProperty] ?? "true");

        string path;
        if (useLocalFile)
        {
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            return new Configuration
            {
                UseLocalFile = Convert.ToBoolean(useLocalFile),
                DatabasePath = Path.Combine(path, Configuration.DatabaseFilename)
            };
        }

        var basePath = OperatingSystem.IsWindows()
            ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        path = OperatingSystem.IsWindows()
            ? Path.Combine(basePath, "Tasky")
            : Path.Combine(basePath, ".config", "tasky");

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        return new Configuration
        {
            UseLocalFile = Convert.ToBoolean(useLocalFile),
            DatabasePath = Path.Combine(path, Configuration.DatabaseFilename)
        };
    }
}