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
    {//todo: check
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions");

        if (!await _admission.CheckIfAdmissionExists(request.ChangeAdmissionPriorityDto.AdmissionId))
            throw new BadRequest("Provided admission does not exist");

        if (!_admission.CheckIfNewPriorityIsAvailable(request.UserId, request.ChangeAdmissionPriorityDto.Priority))
            throw new BadRequest("New priority is out of range of applicant's admissions");

        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.ChangeAdmissionPriorityDto.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");
        
        var admission = await _admission.GetById(request.ChangeAdmissionPriorityDto.AdmissionId);

        await _helper.RearrangeAdmissions(request.UserId, admission.Priority, request.ChangeAdmissionPriorityDto.Priority);

        admission.Priority = request.ChangeAdmissionPriorityDto.Priority;

        await _admission.UpdateAsync(admission);

        return Unit.Value;
    }
}