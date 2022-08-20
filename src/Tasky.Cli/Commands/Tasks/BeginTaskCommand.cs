using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Core.Domain;
using Tasky.Shared;

namespace Tasky.Cli.Commands.Tasks;

public sealed class BeginTaskCommand : BaseCommand<BeginTaskCommand.Settings>
{
    public BeginTaskCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var data = new Dtos.ChangeTaskStatusRequestDto(settings.Id, settings.BoardName);
        var request = new Requests.ChangeTaskStatus(data, Status.InProgress);
        await Mediator.Send(request);

        return await Handle(async () =>
        {
            var boards = await Mediator.Send(new Requests.ListBoardsWithTasks());
            Writer.ShowBoards(boards);
            return 0;
        });
    }

    

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<BeginTaskCommand>(Settings.CommandName)
            .WithAlias(Settings.CommandAlias)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }
    
    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "begin";
        public const string CommandAlias = "bt";
        public const string CommandDescription = "Marks a task already created as begin informing the id and board";
        public static readonly string[] CommandExample = {"begin", "1", "-b", "shopping"};

        [Description("task id")]
        [CommandArgument(0, "[TASK_ID]")]
        public string Id { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        [DefaultValue("board")]
        public string BoardName { get; init; } = "board";
    }
}