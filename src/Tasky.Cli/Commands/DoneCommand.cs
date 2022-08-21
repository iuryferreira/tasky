using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Core.Domain;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public sealed class DoneCommand : BaseCommand<DoneCommand.Settings>
{
    public DoneCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Dto data;
        Requests.Request request;

        var ids = settings.Id.Split('.');
        if (ids.Length > 1)
        {
            data = new ChangeStepStatusRequestDto(settings.Id, ids[0], settings.BoardName);
            request = new Requests.ChangeStepStatus((ChangeStepStatusRequestDto)data, Status.Done);
        }
        else
        {
            data = new ChangeTaskStatusRequestDto(settings.Id, settings.BoardName);
            request = new Requests.ChangeTaskStatus((ChangeTaskStatusRequestDto)data, Status.Done);
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
        configurator.AddCommand<DoneCommand>(Settings.CommandName)
            .WithAlias(Settings.CommandAlias)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "done";
        public const string CommandAlias = "dn";
        public const string CommandDescription = "Marks a task/step already created as done informing the id and board";
        public static readonly string[] CommandExample = { "done", "1", "-b", "shopping" };

        [Description("task/step id")]
        [CommandArgument(0, "<TASK_ID>")]
        public string Id { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        public string BoardName { get; init; } = "";
    }
}