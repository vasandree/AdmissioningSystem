using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.GetAllUsers;

public record GetAllUsersCommand(int Page, int Size):IRequest<PagedUserDto>;