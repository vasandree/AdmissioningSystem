using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetEducationDocument;

public record GetEducationDocumentCommand(Guid UserId) : IRequest<Unit>;