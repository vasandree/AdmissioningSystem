using MediatR;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetUserRoles;

public record GetUserRolesQuery(string Email) : IRequest<RolesDto>;