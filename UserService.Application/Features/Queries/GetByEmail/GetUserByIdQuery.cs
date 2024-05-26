using Common.Models.Models.Dtos;
using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Queries.GetByEmail;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
