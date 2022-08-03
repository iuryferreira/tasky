﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Tasky.Cli.Commands.Input;
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
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    private static void RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<ListTasksCommand>();
        services.AddScoped<DefaultCommand>();
    }

    public static void ConfigureCommands(IConfigurator configurator)
    {
        ListTasksCommand.Configure(configurator);

        configurator
            .SetApplicationName("tasky")
            .SetApplicationVersion("1.0");
    }
}