using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.EditPriority;

public class EditPriorityCommandHandler : IRequestHandler<EditPriorityCommand, Unit>
{
    private readonly PubSubSender _pubSub;
    private readonly IAdmissionRepository _admission;
    private RpcRequestSender _rpc;

    public EditPriorityCommandHandler(IAdmissionRepository admission, PubSubSender pubSub, RpcRequestSender rpc)
    {
        _admission = admission;
        _pubSub = pubSub;
        _rpc = rpc;
    }

    public async Task<Unit> Handle(EditPriorityCommand request, CancellationToken cancellationToken)
    {
        if (!await _admission.CheckExistence(request.AdmissionId))
            throw new NotFound("Provided admission does not exist");

        var admission = await _admission.GetById(request.AdmissionId);

        if (admission.ManagerId != request.ManagerId)
            throw new Forbidden("You are not a manager of this admission");
        
        
        if (!await _rpc.CheckPriorityAvailable(request.AdmissionId, request.NewPriority))
            throw new BadRequest("New priority is not available for this user");
        
        await _pubSub.EditAdmissionPriority(request.AdmissionId, request.NewPriority);
        
        return Unit.Value;
    }
}