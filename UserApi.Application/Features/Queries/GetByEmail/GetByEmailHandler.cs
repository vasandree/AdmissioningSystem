using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetByEmail;

public class GetByEmailHandler : IRequestHandler<GetByEmailQuery, ApplicationUser>
{
    private readonly IUserRepository _repository;

    public GetByEmailHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationUser?> Handle(GetByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByEmail(request.Email);
    }
}