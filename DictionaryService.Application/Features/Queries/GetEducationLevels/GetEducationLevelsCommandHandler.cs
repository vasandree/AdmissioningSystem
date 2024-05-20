using AutoMapper;
using Common.Models.Dtos;
using DictionaryService.Application.Contracts.Persistence;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetEducationLevels;

public class GetEducationLevelsCommandHandler : IRequestHandler<GetEducationLevelsCommand, List<EducationLevelDto>>
{
    private readonly IEducationLevelRepository _educationLevel;
    private readonly IMapper _mapper;

    public GetEducationLevelsCommandHandler(IEducationLevelRepository educationLevel, IMapper mapper)
    {
        _educationLevel = educationLevel;
        _mapper = mapper;
    }

    public async Task<List<EducationLevelDto>> Handle(GetEducationLevelsCommand request, CancellationToken cancellationToken)
    {
        var educationLevels = await _educationLevel.GetAllAsync();
        
        return educationLevels.Select(x => _mapper.Map<EducationLevelDto>(x)).ToList();
    }
}