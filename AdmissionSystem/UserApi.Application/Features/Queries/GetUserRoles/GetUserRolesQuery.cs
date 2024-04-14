using MediatR;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetUserRoles;

public record GetUserRolesQuery(ApplicationUser User) : IRequest<IList<string>>;