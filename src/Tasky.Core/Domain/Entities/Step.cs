using System.Text.Json.Serialization;
using Tasky.Shared;

namespace Tasky.Core.Domain.Entities;

public class Step
{
    [JsonConstructor]
    public Step(string id, string text, Status status, Priority priority = Priority.Normal)
    {
        Id = id;
        Text = text;
        Status = status;
        Priority = priority;
    }

    public string Id { get; }
    public string Text { get; }
    public Status Status { get; private set; }
    public Priority Priority { get; }


    public void ChangeStatus(Task task, Status status)
    {
        if (status == Status.InProgress) task.ChangeStatus(Status.InProgress);
        Status = status;
    }

    public static Step Create(Task task, string text, Priority priority = Priority.Normal) =>
        new($"{task.Id}.{task.Steps.Count + 1}", text, Status.Todo, priority);
}