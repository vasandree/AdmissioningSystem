using Common.Models.Models.Dtos;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetEducationLevels;

public record GetEducationLevelsCommand() : IRequest<List<EducationLevelDto>>;