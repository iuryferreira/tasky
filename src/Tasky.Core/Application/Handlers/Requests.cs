using MediatR;
using Tasky.Core.Domain.Entities;
using Tasky.Shared;

namespace Tasky.Core.Application.Handlers;

public static class Requests
{
    public record ListBoardsWithTasks : IRequest<IEnumerable<Board>>;

    public record AddTaskOnBoard(Dtos.AddTaskOnBoardRequestDto Data) : IRequest<IEnumerable<Board>>;

    public record AddStepOnTask(Dtos.AddStepOnTaskRequestDto Data) : IRequest<IEnumerable<Board>>;
}