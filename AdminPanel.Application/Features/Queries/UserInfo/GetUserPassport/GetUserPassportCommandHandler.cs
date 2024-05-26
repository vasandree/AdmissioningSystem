using AdminPanel.Application.ServiceBus.Rpc;
using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetUserPassport;

public class GetUserPassportCommandHandler: IRequestHandler<GetUserPassportCommand, Unit>
{
    private readonly RpcRequestSender _rpc;

    public GetUserPassportCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<Unit> Handle(GetUserPassportCommand request, CancellationToken cancellationToken)
    {
        var passport = await _rpc.GetPassport(request.UserId);

        return Unit.Value;
    }
}