using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetHeadMangers;

public class GetHeadManagersHandler : IRequestHandler<GetHeadManagersQuery, IReadOnlyList<ManagerEntity>>
{
    private readonly IManagerRepository _repository;

    public GetHeadManagersHandler(IManagerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ManagerEntity>> Handle(GetHeadManagersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetHeadManagers();
    }
}