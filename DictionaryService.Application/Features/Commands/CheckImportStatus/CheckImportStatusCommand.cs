using DictionaryService.Application.DTOs;
using DictionaryService.Domain.Enums;
using MediatR;

namespace DictionaryService.Application.Features.Commands.CheckImportStatus;

public record CheckImportStatusCommand(DictionaryType? DictionaryType) : IRequest<ImportStatusDto>, IRequest;