using AutoMapper;
using Common.Models.Dtos;
using DictionaryService.Application.Contracts.Persistence;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetDocumentType;

public class GetDocumentTypeCommandHandler : IRequestHandler<GetDocumentTypeCommand, List<EducationDocumentTypeDto>>
{
    private readonly IDocumentTypeRepository _documentType;
    private readonly IMapper _mapper;

    public GetDocumentTypeCommandHandler(IDocumentTypeRepository documentType, IMapper mapper)
    {
        _documentType = documentType;
        _mapper = mapper;
    }

    public async Task<List<EducationDocumentTypeDto>> Handle(GetDocumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var documentTypes = await _documentType.GetAllAsync();

        return documentTypes.Select(x => _mapper.Map<EducationDocumentTypeDto>(x)).ToList();
    }
}