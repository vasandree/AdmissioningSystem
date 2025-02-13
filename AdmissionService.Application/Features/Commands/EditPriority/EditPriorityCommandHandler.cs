using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using Common.Models.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Commands.EditPriority;

public class EditPriorityCommandHandler : IRequestHandler<EditPriorityCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;

    public EditPriorityCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        AdmissionsRearrangeHelper helper)
    {
        _admission = admission;
        _applicant = applicant;
        _helper = helper;
    }

    public async Task<Unit> Handle(EditPriorityCommand request, CancellationToken cancellationToken)
    {
        if (await _admission.CheckClosed(request.UserId))
            throw new BadRequest("You cannot edit info, because your admission is closed");
        
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions");

        if (!await _admission.CheckIfAdmissionExists(request.AdmissionId))
            throw new BadRequest("Provided admission does not exist");

        if (!_admission.CheckIfNewPriorityIsAvailable(request.UserId, request.Priority))
            throw new BadRequest("New priority is out of range of applicant's admissions");

        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");
        
        var admission = await _admission.GetById(request.AdmissionId);

        await _helper.RearrangeAdmissions(request.UserId, admission.Priority, request.Priority);

        admission.Priority = request.Priority;

        await _admission.UpdateAsync(admission);

        return Unit.Value;
    }
}