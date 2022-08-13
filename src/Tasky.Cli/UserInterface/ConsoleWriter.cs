using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Task = Tasky.Core.Domain.Entities.Task;

namespace Tasky.Cli.UserInterface;

public interface IConsoleWriter
{
    void ShowBoards(IEnumerable<Board> boards);
}

public class ConsoleWriter : IConsoleWriter
{
    private readonly IRender _render;

    public ConsoleWriter(IRender render)
    {
        _render = render;
    }

    public void ShowBoards(IEnumerable<Board> boards)
    {
        _render.Print("\n");
        var content = Boards.GetBoards(boards);
        _render.Print(content);
    }

    public static class Boards
    {
        public static Content GetBoards(IEnumerable<Board> boards)
        {
            var output = new Content();
            foreach (var board in boards)
            {
                var boardContent = GetBoard(board);
                output.Add(boardContent);
            }

            return output;
        }

        private static Content GetBoard(Board board)
        {
            var tasksCompleted = board.Tasks.Count(x => x.Status == Status.Done);

            var boardName = new Content().Set(board.Name).Bold().Underline().SpacesBefore(2);
            var tasksStatus = new Content()
                .Set($"[{tasksCompleted}/{board.Quantity}]")
                .EscapeMarkup()
                .Grey()
                .SpacesBefore(1)
                .BreakLine();

            var tasks = Tasks.GetTasks(board.Tasks);
            return new Content().Add(boardName).Add(tasksStatus).Add(tasks);
        }
    }

    public static class Tasks
    {
        public static Content GetTasks(List<Task> tasks)
        {
            var tasksContent = new Content();

            foreach (var task in tasks)
            {
                var id = new Content().Set($"{task.Id}.").Grey().Italic().SpacesBefore(6);
                var icon = GetTaskIcon(task);
                var description = GetTaskDescription(task);
                var steps = Steps.GetSteps(task);
                tasksContent.Add(id.Text).Add(icon.Text).Add(description.Text).Add(steps);
            }

            return tasksContent;
        }

        private static Content GetTaskIcon(Task task)
        {
            var content = new Content();
            var icon = task.Status switch
            {
                Status.InProgress => content.Set("...").Bold().SlateBlue(),
                Status.Done => content.Set("[√]").EscapeMarkup().Green(),
                Status.Todo => content.Set("[ ]").EscapeMarkup().Purple(),
                _ => content.Set("[ ]").EscapeMarkup().Purple()
            };

            return icon.SpacesBefore(1);
        }

        private static Content GetTaskDescription(Task task)
        {
            var taskIsDone = task.Status.Equals(Status.Done);

            return new Content().Set(task.Text)
                .EscapeMarkup()
                .Grey(taskIsDone)
                .SpacesBefore(1)
                .BreakLine();
        }
    }

    public static class Steps
    {
        public static Content GetSteps(Task task)
        {
            var stepsContent = new Content();
            foreach (var step in task.Steps)
            {
                var id = new Content().Set($"{step.Id}").Grey().Italic().SpacesBefore(9);
                var icon = GetStepIcon(step);
                var description = GetStepDescription(step);
                stepsContent.Add(id).Add(icon).Add(description);
            }

            return stepsContent;
        }

        private static Content GetStepDescription(Step step)
        {
            var taskIsDone = step.Status.Equals(Status.Done);

            return new Content().Set(step.Text)
                .EscapeMarkup()
                .Grey(taskIsDone)
                .SpacesBefore(1)
                .BreakLine();
        }

        private static Content GetStepIcon(Step step)
        {
            var content = new Content();
            var icon = step.Status switch
            {
                Status.InProgress => content.Set("...").Bold().SlateBlue(),
                Status.Done => content.Set("[√]").EscapeMarkup().Green(),
                Status.Todo => content.Set("[ ]").EscapeMarkup().Purple(),
                _ => content.Set("[ ]").EscapeMarkup().Purple()
            };

            icon = icon.SpacesBefore(1);
            return icon;
        }
    }
}