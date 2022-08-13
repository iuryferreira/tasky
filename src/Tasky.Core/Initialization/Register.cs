using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Core.Infrastructure;

namespace Tasky.Core.Initialization;

public static class Register
{
    public static void RegisterTaskyCore(this IServiceCollection services)
    {
        services.RegisterHandlers();
        services.RegisterInfrastructure();
    }

    private static void RegisterHandlers(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }

    private static void RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IContext, FileDbContext>();
    }
}