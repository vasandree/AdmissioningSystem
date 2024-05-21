using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.DeleteEducationDocumentInfo;

public class DeleteEducationDocumentInfoCommandHandler : IRequestHandler<DeleteEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;

    public DeleteEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument)
    {
        _educationDocument = educationDocument;
    }

    public async Task<Unit> Handle(DeleteEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _educationDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("You haven't added education document yet");
        }

        var educationDocument = (EducationDocument)(await _educationDocument.GetByUserId(request.UserId))!;

        if (educationDocument.File == null)
        {
            await _educationDocument.DeleteAsync(educationDocument);
        }
        else
        {
            educationDocument.Name = null;
            educationDocument.EducationDocumentType = null;

            await _educationDocument.UpdateAsync(educationDocument);
        }

        return Unit.Value;
    }
}