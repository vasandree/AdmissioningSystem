using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.UpdateAsync;

public class UpdateAsyncHandler<T> : IRequestHandler<UpdateAsyncCommand<T>> where T: class
{
    private readonly IGenericRepository<T> _repository;

    public UpdateAsyncHandler(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateAsyncCommand<T> request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(request.Entity);
    }
}