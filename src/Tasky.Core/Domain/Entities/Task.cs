using System.Text.Json.Serialization;
using Tasky.Shared;

namespace Tasky.Core.Domain.Entities;

public class Task
{
    [JsonConstructor]
    public Task(string id, DateTime createdAt, Status status, Priority priority, List<Step> steps, string text)
    {
        Id = id;
        CreatedAt = createdAt;
        Status = status;
        Steps = steps;
        Text = text;
        Priority = priority;
    }

    public string Id { get; private set; }
    public string Text { get; private set; }
    public Status Status { get; private set; }
    public Priority Priority { get; private set; }

    public List<Step> Steps { get; }
    public DateTime CreatedAt { get; }

    public static Task CreateTaskWithText(string id, string text, Priority priority = Priority.Normal) =>
        new(id, DateTime.Now, Status.Todo, priority, new List<Step>(), text);

    public void AddStep(Step step)
    {
        Steps.Add(step);
    }

    public void ChangeStatus(Status status)
    {
        switch (status)
        {
            case Status.Todo:
                Steps.ForEach(step => step.ChangeStatus(this, Status.Todo));
                break;
            case Status.Done:
                Steps.ForEach(step => step.ChangeStatus(this, Status.Done));
                break;
            case Status.InProgress:
            default:
                break;
        }

        Status = status;
    }

    public void SetId(string id) => Id = id;

    public void SortSteps()
    {
        var count = 1;
        Steps.ForEach(step =>
        {
            step.SetId($"{Id}.{count}");
            count++;
        });
    }

    public void SetText(string text) => Text = text;
    public void SetPriority(Priority priority) => Priority = priority;
}