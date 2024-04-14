using MediatR;

namespace UserApi.Application.Features.Commands.CreateAsync;

public record CreateAsyncCommand<T>(T Entity) : IRequest ;