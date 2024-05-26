using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Admissions.GetUserAdmissions;

public class GetApplicantsAdmissions : IRequestHandler<GetApplicantAdmissionsCommand, Unit>
{

    private readonly RpcRequestSender _rpc;

    public GetApplicantsAdmissions(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<Unit> Handle(GetApplicantAdmissionsCommand request, CancellationToken cancellationToken)
    {
        var admissions = await _rpc.GetApplicanrAdmissions(request.ApplicantId);

        return Unit.Value;
    }
}