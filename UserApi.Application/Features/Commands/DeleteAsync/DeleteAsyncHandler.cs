using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.DeleteAsync;

public class DeleteAsyncHandler<T> : IRequestHandler<DeleteAsyncCommand<T>> where T: class
{
    private readonly IGenericRepository<T> _repository;

    public DeleteAsyncHandler(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteAsyncCommand<T> request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Entity);
    }
}