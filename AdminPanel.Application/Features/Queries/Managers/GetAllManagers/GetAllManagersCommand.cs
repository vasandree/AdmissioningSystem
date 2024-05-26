using MediatR;

namespace AdminPanel.Application.Features.Queries.Managers.GetAllManagers;

public record GetAllManagersCommand () : IRequest<Unit>;