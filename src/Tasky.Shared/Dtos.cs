using FluentValidation;
using FluentValidation.Results;

namespace Tasky.Shared;

public abstract record Dto
{
    public bool Valid { get; private set; }
    public ValidationResult? ValidationResult { get; private set; }

    protected void Validate<T>(T entity, AbstractValidator<T> validator)
    {
        ValidationResult = validator.Validate(entity);
        Valid = ValidationResult.IsValid;
    }
}

public record AddTaskOnBoardRequestDto : Dto
{
    public AddTaskOnBoardRequestDto(string boardName, string text, string stepOf, Priority priority)
    {
        BoardName = boardName;
        Text = text;
        StepOf = stepOf;
        Priority = priority;
        Validate(this, new Validator());
    }

    public string BoardName { get; }
    public string Text { get; }
    public string StepOf { get; }
    public Priority Priority { get; }

    private class Validator : AbstractValidator<AddTaskOnBoardRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.BoardName)
                .NotEmpty().WithMessage(Messages.English.BoardNotEmpty);
            RuleFor(dto => dto.Text)
                .NotEmpty().WithMessage(Messages.English.TaskNotEmpty);
        }
    }
}

public record AddStepOnTaskRequestDto : Dto
{
    public AddStepOnTaskRequestDto(string taskId, string boardName, string text, Priority priority)
    {
        TaskId = taskId;
        BoardName = boardName;
        Text = text;
        Priority = priority;
        Validate(this, new Validator());
    }

    public string TaskId { get; }
    public string BoardName { get; }
    public string Text { get; }
    public Priority Priority { get; }

    private class Validator : AbstractValidator<AddStepOnTaskRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId).NotEmpty();
            RuleFor(dto => dto.Text).NotEmpty();
        }
    }
}

public record ChangeTaskStatusRequestDto : Dto
{
    public ChangeTaskStatusRequestDto(string taskId, string boardName)
    {
        TaskId = taskId;
        BoardName = boardName;
        Validate(this, new Validator());
    }

    public string TaskId { get; init; }
    public string BoardName { get; init; }

    private class Validator : AbstractValidator<ChangeTaskStatusRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId)
                .NotEmpty().WithMessage(Messages.English.TaskIdNotEmpty)
                .NotEqual("0").WithMessage(Messages.English.TaskIdEqualZero);
        }
    }
}

public record ChangeStepStatusRequestDto : Dto
{
    public ChangeStepStatusRequestDto(string stepId, string taskId, string boardName)
    {
        StepId = stepId;
        TaskId = taskId;
        BoardName = boardName;
        Validate(this, new Validator());
    }

    public string StepId { get; init; }
    public string TaskId { get; init; }
    public string BoardName { get; init; }

    private class Validator : AbstractValidator<ChangeStepStatusRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId).NotEmpty();
            RuleFor(dto => dto.StepId).NotEmpty();
        }
    }
}