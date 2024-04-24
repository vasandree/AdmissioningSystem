using MediatR;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetHeadMangers;

public record GetHeadManagersQuery : IRequest<IReadOnlyList<ManagerEntity>>;