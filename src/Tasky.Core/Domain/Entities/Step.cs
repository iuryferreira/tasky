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

    public string Id { get; private set; }
    public string Text { get; private set; }
    public Status Status { get; private set; }
    public Priority Priority { get; private set; }


    public void ChangeStatus(Task task, Status status)
    {
        var otherStepsDoneOrInProgress =
            task.Steps.Any(s => (s.Status is Status.Done or Status.InProgress) && !s.Id.Equals(Id));
        if (status == Status.InProgress || otherStepsDoneOrInProgress) task.ChangeStatus(Status.InProgress);
        Status = status;
    }

    public void SetId(string id) => Id = id;

    public void SetText(string text) => Text = text;
    public void SetPriority(Priority priority) => Priority = priority;

    public static Step Create(Task task, string text, Priority priority = Priority.Normal) =>
        new($"{task.Id}.{task.Steps.Count + 1}", text, Status.Todo, priority);
}