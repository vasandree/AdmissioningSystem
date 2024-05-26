using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.GiveAdmission;

public class GiveAdmissionCommandHandler : IRequestHandler<GiveAdmissionCommand, Unit>
{
    private readonly IBaseManagerRepository _baseManager;
    private readonly IAdmissionRepository _admission;
    private readonly PubSubSender _pubSub;
    private readonly RpcRequestSender _rpc;
    private readonly IManagerRepository _manager;

    public GiveAdmissionCommandHandler(RpcRequestSender rpc, IBaseManagerRepository baseManager, IAdmissionRepository admission, PubSubSender pubSub, IManagerRepository manager)
    {
        _rpc = rpc;
        _baseManager = baseManager;
        _admission = admission;
        _pubSub = pubSub;
        _manager = manager;
    }

    public async Task<Unit> Handle(GiveAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _admission.CheckExistence(request.AdmissionId))
            throw new NotFound("Provided admission does not exist");
        

        if (!await _rpc.NotMyAdmission(request.AdmissionId, request.ManagerToGive))
            throw new BadRequest("Managers cannot take their admissions");

        
        var admission = await _admission.GetById(request.AdmissionId);
        
        if (admission.ManagerId != null)
            throw new BadRequest("This admission has manager");

        if (!await _rpc.CheckStatusClosed(request.AdmissionId))
            throw new BadRequest("This admission is closed");
        
        var manager = await _baseManager.GetById(request.ManagerId);
        
        if (!await _baseManager.CheckIfManager(manager))
        {
            admission.ManagerId = request.ManagerId;
            await _admission.UpdateAsync(admission);
            
            await _pubSub.UpdateManager(admission.Id, request.ManagerId);

            return Unit.Value;
        }
            
        if (!await _rpc.CheckFaculty(admission.Id, await _manager.GetFaculty(manager.Id)))
            throw new Forbidden("You are not a manager of proper faculty");
        
        admission.ManagerId = request.ManagerId;
        await _admission.UpdateAsync(admission);
            
        await _pubSub.UpdateManager(admission.Id, request.ManagerId);

        _pubSub.SendEmailToApplicant(admission.Id);
        
        _pubSub.SendEmailToManager(manager.Email, admission.Id);
        
        return Unit.Value;
    }
}