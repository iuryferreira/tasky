namespace Tasky.Shared;

public class Dtos
{
    public record AddTaskOnBoardRequestDto(string BoardName, string Text);

    public record AddStepOnTaskRequestDto(string TaskId, string BoardName, string Text);
}