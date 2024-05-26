using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.DeleteAdmission;

public class DeleteAdmissionCommandHandler : IRequestHandler<DeleteAdmissionCommand, Unit>
{
    private readonly PubSubSender _pubSub;
    private readonly IAdmissionRepository _admission;
    private readonly RpcRequestSender _rpc;

    public DeleteAdmissionCommandHandler(PubSubSender pubSub, IAdmissionRepository admission, RpcRequestSender rpc)
    {
        _pubSub = pubSub;
        _admission = admission;
        _rpc = rpc;
    }

    public async Task<Unit> Handle(DeleteAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _admission.CheckExistence(request.AdmissionId))
            throw new NotFound("Provided admission does not exist");

        var admission = await _admission.GetById(request.AdmissionId);

        if (admission.ManagerId != request.ManagerId)
            throw new Forbidden("You are not a manager of this admission");


        
        await _admission.DeleteAsync(admission);
        await _pubSub.DeleteAdmission(admission.Id);

        return Unit.Value;
    }
}