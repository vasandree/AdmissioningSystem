using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetManagers;

public class GetMangersHandler : IRequestHandler<GetManagersQuery, IReadOnlyList<ManagerEntity>>
{
    private readonly IManagerRepository _repository;

    public GetMangersHandler(IManagerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ManagerEntity>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetManagers();
    }
}