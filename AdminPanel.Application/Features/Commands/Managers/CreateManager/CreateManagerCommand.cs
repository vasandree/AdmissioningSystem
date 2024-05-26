using MediatR;

namespace AdminPanel.Application.Features.Commands.Managers.CreateManager;

public record CreateManagerCommand( Guid UserId, Guid? FacultyId) : IRequest<Unit>;