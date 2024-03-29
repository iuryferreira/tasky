﻿using MediatR;
using Tasky.Core.Domain.Entities;
using Tasky.Shared;

namespace Tasky.Core.Application.Handlers;

public static class Requests
{
    public abstract record Request;

    public record ListBoardsWithTasks : Request, IRequest<IList<Board>>;

    public record AddTaskOnBoard(AddTaskOnBoardRequestDto Data) : Request, IRequest;

    public record AddStepOnTask(AddStepOnTaskRequestDto Data) : Request, IRequest;

    public record ChangeTaskStatus(ChangeTaskStatusRequestDto Data, Status Status) : Request, IRequest;

    public record ChangeStepStatus(ChangeStepStatusRequestDto Data, Status Status) : Request, IRequest;

    public record ClearDoneTasks : Request, IRequest;

    public record DeleteTask(DeleteTaskRequestDto Data) : Request, IRequest;

    public record DeleteStep(DeleteStepRequestDto Data) : Request, IRequest;

    public record EditTask(EditTaskRequestDto Data) : Request, IRequest;

    public record EditStep(EditStepRequestDto Data) : Request, IRequest;
}