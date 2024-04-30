using DictionaryService.Domain.Enums;
using MediatR;

namespace DictionaryService.Application.Features.Commands.ImportDictionaries;

public record ImportDictionariesCommand(DictionaryType? DictionaryType) : IRequest<Unit>;