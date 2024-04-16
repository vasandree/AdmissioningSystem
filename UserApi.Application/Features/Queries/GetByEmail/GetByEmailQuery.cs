using MediatR;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetByEmail;

public record GetByEmailQuery(string Email) : IRequest<UserDto>;
