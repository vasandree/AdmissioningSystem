using AdminPanel.Application.ServiceBus.Rpc;
using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetUserInfo;

public class GetUserInfoCommandHandler : IRequestHandler<GetUserInfoCommand, Unit>
{
    private readonly RpcRequestSender _rpc;

    public GetUserInfoCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<Unit> Handle(GetUserInfoCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _rpc.GetUserInfo(request.ApplicantId);

        return Unit.Value;
    }
}