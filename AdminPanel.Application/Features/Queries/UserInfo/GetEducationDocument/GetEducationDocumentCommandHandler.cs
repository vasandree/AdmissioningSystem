using AdminPanel.Application.ServiceBus.Rpc;
using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetEducationDocument;

public class GetEducationDocumentCommandHandler : IRequestHandler<GetEducationDocumentCommand, Unit>
{
    private readonly RpcRequestSender _rpc;

    public GetEducationDocumentCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<Unit> Handle(GetEducationDocumentCommand request, CancellationToken cancellationToken)
    {
        var educationDoc = await _rpc.GetEducationDocument(request.UserId);

        return Unit.Value;
    }
}