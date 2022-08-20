using FluentValidation;
using FluentValidation.Results;

namespace Tasky.Shared;

public static class Dtos
{
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
        public AddTaskOnBoardRequestDto(string boardName, string text)
        {
            BoardName = boardName;
            Text = text;
            Validate(this, new Validator());
        }

        public string BoardName { get; init; }
        public string Text { get; init; }

        private class Validator : AbstractValidator<AddTaskOnBoardRequestDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Text).NotEmpty();
            }
        }
    }

    public record AddStepOnTaskRequestDto : Dto
    {
        public AddStepOnTaskRequestDto(string taskId, string boardName, string text)
        {
            TaskId = taskId;
            BoardName = boardName;
            Text = text;
            Validate(this, new Validator());
        }

        public string TaskId { get; init; }
        public string BoardName { get; init; }
        public string Text { get; init; }

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
}