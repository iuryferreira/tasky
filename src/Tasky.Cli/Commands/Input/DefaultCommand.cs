using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.Commands.Output;
using Tasky.Cli.Contracts;

namespace Tasky.Cli.Commands.Input;

public sealed class DefaultCommand : ListTasksCommand
{
    public DefaultCommand(IMediator mediator) : base(mediator)
    {
    }
}

public class ListTasksCommand : BaseCommand<ListTasksCommand.Settings>
{
    protected ListTasksCommand(IMediator mediator) : base(mediator)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        await Mediator.Send(new Requests.ListTaskOutputRequest());
        return await Task.FromResult(0);
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<ListTasksCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    public class Settings : CommandSettings
    {
        public const string CommandName = "list";
        public const string CommandDescription = "List all tasks of all boards";
        public static readonly string[] CommandExample = {"list", "-i"};
    }
}