using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Tasky.Core.Initialization;

public static class Register
{
    public static void RegisterTaskyCore(this IServiceCollection services)
    {
        services.RegisterHandlers();
    }

    private static void RegisterHandlers(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}