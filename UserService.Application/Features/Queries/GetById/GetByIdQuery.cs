using MediatR;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Features.Queries.GetById;

public record GetByIdQuery<T>(Guid Id) : IRequest<T> where T: class;