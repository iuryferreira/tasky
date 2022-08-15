using MediatR;
using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Tasky.Shared;

namespace Tasky.Core.Application.Handlers;

public static class Requests
{
    public record ListBoardsWithTasks : IRequest<IEnumerable<Board>>;

    public record AddTaskOnBoard(Dtos.AddTaskOnBoardRequestDto Data) : IRequest;

    public record AddStepOnTask(Dtos.AddStepOnTaskRequestDto Data) : IRequest;

    public record ChangeTaskStatus(Dtos.ChangeTaskStatusRequestDto Data, Status Status) : IRequest;

    public record ChangeStepStatus(Dtos.ChangeStepStatusRequestDto Data, Status Status) : IRequest;
}