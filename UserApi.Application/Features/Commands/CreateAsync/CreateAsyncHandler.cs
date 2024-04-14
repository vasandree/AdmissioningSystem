using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.CreateAsync;

public class CreateAsyncHandler<T> : IRequestHandler<CreateAsyncCommand<T>> where T: class
{
    private readonly IGenericRepository<T> _repository;

    public CreateAsyncHandler(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateAsyncCommand<T> request, CancellationToken cancellationToken)
    {
        await _repository.CreateAsync(request.Entity);
    }
}