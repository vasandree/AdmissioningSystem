using MediatR;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetManagers;

public record GetManagersQuery : IRequest<IReadOnlyList<ManagerEntity>>;
