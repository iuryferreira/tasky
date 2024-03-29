﻿using Notie.Models;
using Spectre.Console;
using Tasky.Core.Domain.Entities;
using Tasky.Shared;
using Status = Tasky.Shared.Status;
using Task = Tasky.Core.Domain.Entities.Task;

namespace Tasky.Cli.UserInterface;

public interface IConsoleWriter
{
    void ShowBoards(IList<Board> boards);
    void ShowErrors(IEnumerable<Notification> notifications);
}

public class ConsoleWriter : IConsoleWriter
{
    private readonly IRender _render;

    public ConsoleWriter(IRender render)
    {
        _render = render;
    }

    public void ShowBoards(IList<Board> boards)
    {
        if (boards.Count.Equals(0))
        {
            var notification =
                new Notification("Boards", "There are no boards created. Add your first task to see them.");
            ShowErrors(new[] {notification});
            return;
        }

        _render.Print("\n");
        var content = Boards.GetBoards(boards);
        _render.Print(content);
    }

    public void ShowErrors(IEnumerable<Notification> notifications)
    {
        _render.Print("\n");
        foreach (var notification in notifications)
        {
            var content = new Content().Set($"{Emoji.Known.RedCircle} {notification.Message}").BreakLine();
            _render.Print(content);
        }
    }

    private static class Boards
    {
        public static Content GetBoards(IList<Board> boards)
        {
            var output = new Content();
            foreach (var boardContent in boards.Select(board => GetBoard(board).BreakLine())) output.Add(boardContent);
            output.Add(GetBoardsStatistics(boards)).BreakLine(2);
            return output;
        }

        private static Content GetBoard(Board board)
        {
            var tasksCompleted = board.Tasks.Count(x => x.Status == Status.Done);
            var boardName = new Content().Set($"@{board.Name}").Bold().Underline().SpacesBefore(2);
            var tasksStatus = new Content()
                .Set($"[{tasksCompleted}/{board.TasksQuantity}]")
                .EscapeMarkup()
                .Grey()
                .SpacesBefore(1)
                .BreakLine(2);

            var tasks = Tasks.GetTasks(board.Tasks);
            return new Content().Add(boardName).Add(tasksStatus).Add(tasks);
        }

        private static Content GetBoardsStatistics(IEnumerable<Board> boards)
        {
            var tasks = boards.SelectMany(x => x.Tasks).ToList();
            var tasksDoneCount = tasks.Count(x => x.Status.Equals(Status.Done));
            var tasksInProgressCount = tasks.Count(x => x.Status.Equals(Status.InProgress));
            var tasksTodoCount = tasks.Count(x => x.Status.Equals(Status.Todo));

            var percentageValue = Math.Ceiling(tasksDoneCount / (decimal) tasks.Count * 100);
            var percentageContent = new Content()
                .Set($"{percentageValue}%")
                .SpacesBefore(1)
                .Yellow();

            var progress = new Content()
                .SpacesBefore(1)
                .Add($"{percentageContent} ")
                .Add(Messages.English.StatisticsProgress)
                .Grey()
                .BreakLine();
            var status = new Content()
                .Add($"{tasksDoneCount} {Messages.English.StatisticsDone} · ")
                .Green()
                .SpacesBefore(2)
                .Add($"{tasksInProgressCount} {Messages.English.StatisticsInProgress} · ")
                .SlateBlue()
                .Add($"{tasksTodoCount} {Messages.English.StatisticsPending}")
                .Purple();

            return new Content().Add(progress).Add(status);
        }
    }

    private static class Tasks
    {
        public static Content GetTasks(IEnumerable<Task> tasks)
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

            var taskContent = new Content().Set(task.Text)
                .EscapeMarkup()
                .Dimmed(taskIsDone);

            var priorityContent = new Content();

            switch (task.Priority)
            {
                case Priority.High:
                    taskContent
                        .Red(!taskIsDone)
                        .Bold(!taskIsDone)
                        .Underline(!taskIsDone)
                        .StrikeThrough(taskIsDone);

                    priorityContent.Set("(!!)")
                        .Bold(!taskIsDone)
                        .Dimmed(taskIsDone)
                        .Red(!taskIsDone)
                        .StrikeThrough(taskIsDone)
                        .SpacesBefore(1);
                    break;
                case Priority.Medium:
                    taskContent
                        .Yellow(!taskIsDone)
                        .Underline(!taskIsDone)
                        .StrikeThrough(taskIsDone);
                    priorityContent.Set("(!)")
                        .Yellow(!taskIsDone)
                        .Dimmed(taskIsDone)
                        .StrikeThrough(taskIsDone)
                        .SpacesBefore(1);
                    break;
                case Priority.Normal:
                default:
                    break;
            }

            return taskContent.Add(priorityContent).SpacesBefore(1).BreakLine();
        }
    }

    private static class Steps
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
            var stepIsDone = step.Status.Equals(Status.Done);

            var stepContent = new Content().Set(step.Text)
                .EscapeMarkup()
                .Dimmed(stepIsDone);

            var priorityContent = new Content();

            switch (step.Priority)
            {
                case Priority.High:
                    stepContent
                        .Red(!stepIsDone)
                        .Bold(!stepIsDone)
                        .Underline(!stepIsDone)
                        .StrikeThrough(stepIsDone);
                    priorityContent.Set("(!!)")
                        .Bold(!stepIsDone)
                        .Red(!stepIsDone)
                        .Dimmed(stepIsDone)
                        .StrikeThrough(stepIsDone)
                        .SpacesBefore(1);
                    break;
                case Priority.Medium:
                    stepContent
                        .Yellow(!stepIsDone)
                        .Underline(!stepIsDone)
                        .StrikeThrough(stepIsDone);
                    priorityContent.Set("(!)")
                        .Yellow(!stepIsDone)
                        .Dimmed(stepIsDone)
                        .StrikeThrough(stepIsDone)
                        .SpacesBefore(1);
                    break;
                case Priority.Normal:
                default:
                    break;
            }

            return stepContent.Add(priorityContent).SpacesBefore(1).BreakLine();
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

            return icon.SpacesBefore(1);
        }
    }
}