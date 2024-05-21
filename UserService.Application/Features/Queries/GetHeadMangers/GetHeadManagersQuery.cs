using MediatR;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Features.Queries.GetHeadMangers;

public record GetHeadManagersQuery : IRequest<IReadOnlyList<ManagerEntity>>;