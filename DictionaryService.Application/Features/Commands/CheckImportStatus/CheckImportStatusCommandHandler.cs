using DictionaryService.Application.DTOs;
using MediatR;

namespace DictionaryService.Application.Features.Commands.CheckImportStatus;

public class CheckImportStatusCommandHandler : IRequestHandler<CheckImportStatusCommand, ImportStatusDto>
{
    private readonly ImportTaskTracker _importTaskTracker;

    public CheckImportStatusCommandHandler(ImportTaskTracker importTaskTracker)
    {
        _importTaskTracker = importTaskTracker;
    }

    public Task<ImportStatusDto> Handle(CheckImportStatusCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ImportStatusDto()
        {
            Status = _importTaskTracker.GetTaskStatus(request.TaskId)
        });
    }
}