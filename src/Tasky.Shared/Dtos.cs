namespace Tasky.Shared;

public static class Dtos
{
    public record AddTaskOnBoardRequestDto(string BoardName, string Text);

    public record AddStepOnTaskRequestDto(string TaskId, string BoardName, string Text);

    public record ChangeTaskStatusRequestDto(string TaskId, string BoardName);

    public record ChangeStepStatusRequestDto(string StepId, string TaskId, string BoardName);
}