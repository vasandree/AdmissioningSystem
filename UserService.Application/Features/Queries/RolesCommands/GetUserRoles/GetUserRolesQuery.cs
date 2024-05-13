using MediatR;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Queries.RolesCommands.GetUserRoles;

public record GetUserRolesQuery(Guid Id) : IRequest<RolesDto>;