using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using AdmissionService.Application.PubSub.Senders;
using Common.Models.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Commands.DeleteAdmission;

public class DeleteAdmissionCommandHandler : IRequestHandler<DeleteAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;
    private readonly PubSubSender _pubSub;

    public DeleteAdmissionCommandHandler(AdmissionsRearrangeHelper helper, IApplicantRepository applicant, IAdmissionRepository admission, PubSubSender pubSub)
    {
        _helper = helper;
        _applicant = applicant;
        _admission = admission;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(DeleteAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions");
        
        if (!await _admission.CheckIfAdmissionExists(request.AdmissionId))
            throw new BadRequest("Provided admission does not exist");
        //todo: check
        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");

        var admission = await _admission.GetById(request.AdmissionId);
        await _admission.DeleteAsync(admission);
        await _helper.RearrangeAdmissionsByDeletion(request.UserId, admission.Priority);
        
        if(!await _applicant.CheckIfApplicantHasAdmissions(request.UserId))
            _pubSub.UpdateApplicantRole(request.UserId);
        //todo: check
        
        return Unit.Value;
    }
}