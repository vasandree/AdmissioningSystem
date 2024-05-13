using MediatR;

namespace UserService.Application.Features.Queries.GetAllAsync;

public record GetAllAsyncQuery<T>() : IRequest<IReadOnlyList<T>> where T : class;