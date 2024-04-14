using MediatR;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetByEmail;

public record GetByEmailQuery(string Email) : IRequest<ApplicationUser>;
