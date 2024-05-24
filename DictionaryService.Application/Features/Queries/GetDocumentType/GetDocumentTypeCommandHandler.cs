using AutoMapper;
using Common.Models.Models.Dtos;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetDocumentType;

public class GetDocumentTypeCommandHandler : IRequestHandler<GetDocumentTypeCommand, List<EducationDocumentTypeDto>>
{
    private readonly IDocumentTypeRepository _documentType;
    private readonly INextEducationLevelRepository _nextEducationLevel;
    private readonly IMapper _mapper;

    public GetDocumentTypeCommandHandler(IDocumentTypeRepository documentType, IMapper mapper,
        INextEducationLevelRepository nextEducationLevel)
    {
        _documentType = documentType;
        _mapper = mapper;
        _nextEducationLevel = nextEducationLevel;
    }

    public async Task<List<EducationDocumentTypeDto>> Handle(GetDocumentTypeCommand request,
        CancellationToken cancellationToken)
    {
        var documentTypes = await _documentType.GetAllAsync();
        var dtos = new List<EducationDocumentTypeDto>();

        foreach (var documentType in documentTypes)
        {
            var nextEducationLevels = await _nextEducationLevel.GetEducationLevels(documentType.Id);

            var dto = _mapper.Map<EducationDocumentTypeDto>(documentType);
            
            dto.NextEducationLevels = nextEducationLevels.Select(x => _mapper.Map<EducationLevelDto>(x)).ToList();
            
            dtos.Add(dto);
        }

        return dtos;
    }
}