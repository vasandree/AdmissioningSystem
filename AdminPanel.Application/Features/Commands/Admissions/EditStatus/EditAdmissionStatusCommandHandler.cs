using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.EditStatus;

public class EditAdmissionStatusCommandHandler : IRequestHandler<EditAdmissionStatusCommand, Unit>
{
    private readonly PubSubSender _pubSub;
    private readonly IAdmissionRepository _admission;
    private RpcRequestSender _rpc;

    public EditAdmissionStatusCommandHandler(IAdmissionRepository admission, PubSubSender pubSub, RpcRequestSender rpc)
    {
        _admission = admission;
        _pubSub = pubSub;
        _rpc = rpc;
    }

    public async Task<Unit> Handle(EditAdmissionStatusCommand request, CancellationToken cancellationToken)
    {
        if(request.Status == AdmissionStatus.UnderConsideration || request.Status == AdmissionStatus.Created)
            throw new BadRequest("Invalid status for this admission");
        
        if (!await _admission.CheckExistence(request.AdmissionId))
            throw new NotFound("Provided admission does not exist");

        var admission = await _admission.GetById(request.AdmissionId);

        if (admission.ManagerId != request.ManagerId)
            throw new Forbidden("You are not a manager of this admission");
        
        await _pubSub.UpdateStatus(request.AdmissionId, request.Status);
        
        return Unit.Value;
    }
    
}