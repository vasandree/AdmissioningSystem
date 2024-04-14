using MediatR;

namespace UserApi.Application.Features.Commands.UpdateAsync;

public record UpdateAsyncCommand<T>(T Entity) : IRequest;