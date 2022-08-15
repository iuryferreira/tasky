using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;

namespace Tasky.Cli.Commands.Boards;

public class ShowBoardsCommand : BaseCommand<ShowBoardsCommand.Settings>
{
    protected ShowBoardsCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var boards = await Mediator.Send(new Requests.ListBoardsWithTasks());
        Writer.ShowBoards(boards);
        return await Task.FromResult(0);
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<ShowBoardsCommand>(Settings.CommandName)
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