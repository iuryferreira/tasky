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
            RuleFor(dto => dto.BoardName).NotEmpty().WithMessage(Messages.English.BoardNotEmpty);
            RuleFor(dto => dto.Text).NotEmpty().WithMessage(Messages.English.TaskNotEmpty);
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
            RuleFor(dto => dto.TaskId).NotEmpty().WithMessage(Messages.English.TaskIdNotEmpty);
            RuleFor(dto => dto.Text).NotEmpty().WithMessage(Messages.English.TaskNotEmpty);
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

    public string TaskId { get; }
    public string BoardName { get; }

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

    public string StepId { get; }
    public string TaskId { get; }
    public string BoardName { get; }

    private class Validator : AbstractValidator<ChangeStepStatusRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId).NotEmpty().WithMessage(Messages.English.TaskIdNotEmpty);
            RuleFor(dto => dto.StepId).NotEmpty().WithMessage(Messages.English.StepIdNotEmpty);
        }
    }
}

public record DeleteTaskRequestDto : Dto
{
    public DeleteTaskRequestDto(string taskId, string boardName)
    {
        TaskId = taskId;
        BoardName = boardName;
        Validate(this, new Validator());
    }

    public string TaskId { get; }
    public string BoardName { get; }

    private class Validator : AbstractValidator<DeleteTaskRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId).NotEmpty().WithMessage(Messages.English.TaskIdNotEmpty);
            RuleFor(dto => dto.BoardName).NotEmpty().WithMessage(Messages.English.BoardNotEmpty);
        }
    }
}

public record DeleteStepRequestDto : DeleteTaskRequestDto
{
    public DeleteStepRequestDto(string stepId, string taskId, string boardName) : base(taskId, boardName)
    {
        StepId = stepId;
        Validate(this, new Validator());
    }

    public string StepId { get; }

    private class Validator : AbstractValidator<DeleteStepRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.StepId).NotEmpty().WithMessage(Messages.English.StepIdNotEmpty);
        }
    }
}

public abstract record EditRequestDto : Dto
{
    protected EditRequestDto(string taskId, string boardName, string? text, Priority? priority)
    {
        TaskId = taskId;
        BoardName = boardName;
        Text = text;
        Priority = priority;
        Validate(this, new Validator());
    }

    public string TaskId { get; }
    public string BoardName { get; }
    public string? Text { get; }
    public Priority? Priority { get; }

    private class Validator : AbstractValidator<EditRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.TaskId).NotEmpty().WithMessage(Messages.English.TaskIdNotEmpty);
            RuleFor(dto => dto.BoardName).NotEmpty().WithMessage(Messages.English.BoardNotEmpty);
        }
    }
}

public record EditTaskRequestDto : EditRequestDto
{
    public EditTaskRequestDto(string taskId, string boardName, string? text, Priority? priority) : base(taskId,
        boardName, text, priority)
    {
    }
}

public record EditStepRequestDto : EditRequestDto
{
    public EditStepRequestDto(string taskId, string boardName, string stepId, string? text, Priority? priority) : base(
        taskId, boardName, text, priority)
    {
        StepId = stepId;
        Validate(this, new Validator());
    }

    public string StepId { get; }

    private class Validator : AbstractValidator<EditStepRequestDto>
    {
        public Validator()
        {
            RuleFor(dto => dto.StepId).NotEmpty().WithMessage(Messages.English.StepIdNotEmpty);
        }
    }
}