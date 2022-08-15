using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Tasky.Cli.Commands;
using Tasky.Cli.Commands.Boards;
using Tasky.Cli.Commands.Steps;
using Tasky.Cli.Commands.Tasks;
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
        services.AddScoped<ShowBoardsCommand>();

        services.AddScoped<AddTaskCommand>();
        services.AddScoped<DoneTaskCommand>();

        services.AddScoped<AddStepCommand>();
        services.AddScoped<DoneStepCommand>();

        services.AddScoped<DefaultCommand>();
    }

    public static void ConfigureCommands(IConfigurator configurator)
    {
        AddTaskCommand.Configure(configurator);
        DoneTaskCommand.Configure(configurator);

        AddStepCommand.Configure(configurator);
        DoneStepCommand.Configure(configurator);

        ShowBoardsCommand.Configure(configurator);


        configurator
            .SetApplicationName("tasky")
            .SetApplicationVersion("1.0");
    }
}