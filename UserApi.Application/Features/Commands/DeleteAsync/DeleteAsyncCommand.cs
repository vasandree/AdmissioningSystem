using MediatR;

namespace UserApi.Application.Features.Commands.DeleteAsync;

public record DeleteAsyncCommand<T>(T Entity) : IRequest;
