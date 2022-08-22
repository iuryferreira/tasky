using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public class DeleteCommand : BaseCommand<DeleteCommand.Settings>
{
    public DeleteCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer,
        notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Dto data;
        Requests.Request request;

        if (settings.Clear)
        {
            await Mediator.Send(new Requests.ClearDoneTasks());
            return await Handle(async () =>
            {
                var boards = await Mediator.Send(new Requests.ListBoardsWithTasks());
                Writer.ShowBoards(boards);
                return 0;
            });
        }

        var ids = settings.Id.Split('.');
        if (ids.Length > 1)
        {
            data = new DeleteStepRequestDto(settings.Id, ids[0], settings.BoardName);
            request = new Requests.DeleteStep((DeleteStepRequestDto) data);
        }
        else
        {
            data = new DeleteTaskRequestDto(settings.Id, settings.BoardName);
            request = new Requests.DeleteTask((DeleteTaskRequestDto) data);
        }

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
        configurator.AddCommand<DeleteCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "delete";
        public const string CommandDescription = Messages.English.Delete;
        public static readonly string[] CommandExample = {"delete", "1", "-b", "dev"};

        [Description("task/step id")]
        [CommandArgument(0, "[TASK_ID]")]
        public string Id { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        public string BoardName { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-c|--clear")]
        public bool Clear { get; init; }
    }
}