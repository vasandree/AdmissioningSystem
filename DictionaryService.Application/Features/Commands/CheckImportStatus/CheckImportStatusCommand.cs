using DictionaryService.Application.DTOs;
using DictionaryService.Domain.Enums;
using MediatR;

namespace DictionaryService.Application.Features.Commands.CheckImportStatus;

public record CheckImportStatusCommand(Guid TaskId) : IRequest<ImportStatusDto>;