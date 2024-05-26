using AdminPanel.Application.ServiceBus.Rpc;
using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetFile;

public class GetFileCommandHandler : IRequestHandler<GetFileCommand, Unit>
{
    private readonly RpcRequestSender _rpc;

    public GetFileCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<Unit> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _rpc.GetFile(request.ApplicantId, request.DocumentType);

        return Unit.Value;
    }
}