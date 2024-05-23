using Common.Services.Repository;
using MediatR;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Queries.GetAllAsync;

public class GetAllAsyncHandler<T> : IRequestHandler<GetAllAsyncQuery<T>, IReadOnlyList<T>> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GetAllAsyncHandler(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<T>> Handle(GetAllAsyncQuery<T> request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}