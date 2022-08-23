using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public class EditCommand : BaseCommand<EditCommand.Settings>
{
    public EditCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Dto data;
        Requests.Request request;

        var ids = settings.Id.Split('.');
        if (ids.Length > 1)
        {
            data = new EditStepRequestDto(ids[0], settings.BoardName, settings.Id, settings.Text, settings.Priority);
            request = new Requests.EditStep((EditStepRequestDto) data);
        }
        else
        {
            data = new EditTaskRequestDto(settings.Id, settings.BoardName, settings.Text, settings.Priority);
            request = new Requests.EditTask((EditTaskRequestDto) data);
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
        configurator.AddCommand<EditCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "edit";
        public const string CommandDescription = Messages.English.Edit;
        public static readonly string[] CommandExample = {"edit", "1", "-b", "dev"};

        public Settings(string id, string boardName, string? text, Priority? priority)
        {
            Id = id;
            BoardName = boardName;
            Text = text;
            Priority = priority;
        }

        [Description("task/step id")]
        [CommandArgument(0, "[TASK_ID]")]
        public string Id { get; }

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        public string BoardName { get; }

        [Description("set task/step content")]
        [CommandOption("-t|--text")]
        public string? Text { get; }

        [Description("set task/step priority")]
        [CommandOption("-p|--priority")]
        public Priority? Priority { get; }
    }
}