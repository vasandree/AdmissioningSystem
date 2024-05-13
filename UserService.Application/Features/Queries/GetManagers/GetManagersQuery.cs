using MediatR;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Features.Queries.GetManagers;

public record GetManagersQuery : IRequest<IReadOnlyList<ManagerEntity>>;
