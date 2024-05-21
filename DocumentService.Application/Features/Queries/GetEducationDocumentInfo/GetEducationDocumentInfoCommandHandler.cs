using AutoMapper;
using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Responses;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetEducationDocumentInfo;

public class GetEducationDocumentInfoCommandHandler : IRequestHandler<GetEducationDocumentInfoCommand, EducationDocumentDto>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly IMapper _mapper;

    public GetEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument, IMapper mapper)
    {
        _educationDocument = educationDocument;
        _mapper = mapper;
    }

    public async Task<EducationDocumentDto> Handle(GetEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _educationDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("Education Document info for this user does not exist");
        }

        var educationDocument = (EducationDocument)(await _educationDocument.GetByUserId(request.UserId))!;

        if (educationDocument.Name == null || educationDocument.EducationDocumentType == null)
        {
            throw new BadRequest("Education Document info for this user does not exist");

        }

        return _mapper.Map<EducationDocumentDto>(educationDocument);
    }
}