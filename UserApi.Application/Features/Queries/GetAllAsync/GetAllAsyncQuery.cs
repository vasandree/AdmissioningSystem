using MediatR;

namespace UserApi.Application.Features.Queries.GetAllAsync;

public record GetAllAsyncQuery<T>() : IRequest<IReadOnlyList<T>> where T : class;