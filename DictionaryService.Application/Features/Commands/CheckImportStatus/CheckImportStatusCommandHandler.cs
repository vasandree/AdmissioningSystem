using DictionaryService.Application.DTOs;
using MediatR;

namespace DictionaryService.Application.Features.Commands.CheckImportStatus;

public class CheckImportStatusCommandHandler : IRequestHandler<CheckImportStatusCommand, ImportStatusDto>
{
    public Task<ImportStatusDto> Handle(CheckImportStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}