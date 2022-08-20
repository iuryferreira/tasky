using MediatR;
using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Tasky.Shared;

namespace Tasky.Core.Application.Handlers;

public static class Requests
{
    public abstract record Request; 
    
    public record ListBoardsWithTasks : Request, IRequest<IEnumerable<Board>>;

    public record AddTaskOnBoard(Dtos.AddTaskOnBoardRequestDto Data) : Request, IRequest;

    public record AddStepOnTask(Dtos.AddStepOnTaskRequestDto Data) : Request, IRequest;

    public record ChangeTaskStatus(Dtos.ChangeTaskStatusRequestDto Data, Status Status) : Request, IRequest;

    public record ChangeStepStatus(Dtos.ChangeStepStatusRequestDto Data, Status Status) : Request, IRequest;
}