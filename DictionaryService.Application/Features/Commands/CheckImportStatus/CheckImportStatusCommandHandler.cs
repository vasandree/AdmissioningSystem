using MediatR;

namespace DictionaryService.Application.Features.Commands.CheckImportStatus;

public class CheckImportStatusCommandHandler : IRequestHandler<CheckImportStatusCommand>
{
    public Task Handle(CheckImportStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}