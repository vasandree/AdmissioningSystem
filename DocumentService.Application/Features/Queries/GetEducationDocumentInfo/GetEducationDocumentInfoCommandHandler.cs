using AutoMapper;
using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Responses;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetEducationDocumentInfo;

public class GetEducationDocumentInfoCommandHandler : IRequestHandler<GetEducationDocumentInfoCommand, EducationDocumentDto>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly RpcRequestSender _rpc;
    private readonly IMapper _mapper;

    public GetEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument, IMapper mapper, RpcRequestSender rpc)
    {
        _educationDocument = educationDocument;
        _mapper = mapper;
        _rpc = rpc;
    }

    public async Task<EducationDocumentDto> Handle(GetEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        var educationDocument = (EducationDocument)await _educationDocument.GetByUserId(request.UserId);
        if (educationDocument == null)
        {
            throw new BadRequest("Education Document info for this user does not exist");
        }

        if (string.IsNullOrEmpty(educationDocument.Name) || educationDocument.EducationDocumentTypeId == null)
        {
            throw new BadRequest("Education Document info for this user does not exist");
        }

        var dto = _mapper.Map<EducationDocumentDto>(educationDocument);

        var documentType = await _rpc.GetDocumentTypeDto(educationDocument.EducationDocumentTypeId.Value);
        if (documentType.EducationDocumentTypeDto == null)
        {
            throw new BadRequest("Failed to retrieve the document type information");
        }

        dto.EducationDocumentType = documentType.EducationDocumentTypeDto;
        return dto;
    }
}