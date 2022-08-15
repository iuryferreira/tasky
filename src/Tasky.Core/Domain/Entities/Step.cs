using System.Text.Json.Serialization;

namespace Tasky.Core.Domain.Entities;

public class Step
{
    [JsonConstructor]
    public Step(string id, string text, Status status)
    {
        Id = id;
        Text = text;
        Status = status;
    }

    public string Id { get; }
    public string Text { get; }
    public Status Status { get; private set; }

    public void ChangeStatus(Status status)
    {
        Status = status;
    }
}