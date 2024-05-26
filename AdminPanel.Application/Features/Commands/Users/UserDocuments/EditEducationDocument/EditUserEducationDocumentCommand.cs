using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserDocuments.EditEducationDocument;

public record EditUserEducationDocumentCommand(Guid ManagerId, Guid UserId, string Name) : IRequest<Unit>;