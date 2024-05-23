using Common.Models.Models.Dtos;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetDocumentType;

public record GetDocumentTypeCommand(): IRequest<List<EducationDocumentTypeDto>>, IRequest<EducationDocumentTypeDto>;