using System.Text.Json.Serialization;

namespace Tasky.Core.Domain.Entities;

public class Task
{
    [JsonConstructor]
    public Task(string id, DateTime createdAt, Status status, List<Step> steps, string text)
    {
        Id = id;
        CreatedAt = createdAt;
        Status = status;
        Steps = steps;
        Text = text;
    }

    public string Id { get; }
    public string Text { get; }
    public Status Status { get; }
    public List<Step> Steps { get; } = new();
    public DateTime CreatedAt { get; set; }


    public static Task CreateTaskWithText(string id, string text) =>
        new(id, DateTime.Now, Status.Todo, new List<Step>(), text);

    public void AddStep(Step step)
    {
        Steps.Add(step);
    }
}