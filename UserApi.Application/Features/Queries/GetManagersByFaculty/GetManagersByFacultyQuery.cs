using MediatR;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetManagersByFaculty;

public record GetManagersByFacultyQuery(Guid Id) : IRequest<IReadOnlyList<ManagerEntity>>;