using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Queries.GetUserRoles;

public class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, IList<string>>
{
    private readonly IUserRepository _repository;

    public GetUserRolesHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUserRoles(request.User);
    }
}