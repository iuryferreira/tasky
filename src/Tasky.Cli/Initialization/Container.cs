using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Cli.Commands;
using Tasky.Cli.UserInterface;
using Tasky.Core.Initialization;

namespace Tasky.Cli.Initialization;

public static class Container
{
    public static ServiceProvider Provider { get; private set; } = new ServiceCollection().BuildServiceProvider();
    private static ServiceCollection Services { get; } = new();
    public static TypeRegister? Register { get; private set; }

    public static void BuildProvider()
    {
        Services.RegisterTaskyCore();
        ConfigureServices(Services);
        Provider = Services.BuildServiceProvider();
        Register = new TypeRegister(Services);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterCommands();
        services.AddScoped<IRender, Render>();
        services.AddScoped<IConsoleWriter, ConsoleWriter>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    private static void RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<ListCommand>();

        services.AddScoped<AddCommand>();
        services.AddScoped<DoneCommand>();
        services.AddScoped<BeginCommand>();
        services.AddScoped<ResetCommand>();
        services.AddScoped<DefaultCommand>();
    }

    public static void ConfigureCommands(IConfigurator configurator)
    {
        AddCommand.Configure(configurator);
        DoneCommand.Configure(configurator);
        BeginCommand.Configure(configurator);
        ResetCommand.Configure(configurator);
        ListCommand.Configure(configurator);

        configurator
            .SetApplicationName("tasky")
            .SetApplicationVersion("1.0");
    }
}