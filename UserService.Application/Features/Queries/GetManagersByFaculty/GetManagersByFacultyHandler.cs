using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Queries.GetManagersByFaculty;

public class GetManagersByFacultyHandler : IRequestHandler<GetManagersByFacultyQuery, IReadOnlyList<ManagerEntity>>
{
    private readonly IManagerRepository _repository;

    public GetManagersByFacultyHandler(IManagerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ManagerEntity>> Handle(GetManagersByFacultyQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetManagersByFaculty(request.Id);
    }
}