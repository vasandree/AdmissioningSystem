using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using Common.Models.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Commands.DeleteAdmission;

public class DeleteAdmissionCommandHandler : IRequestHandler<DeleteAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;

    public DeleteAdmissionCommandHandler(AdmissionsRearrangeHelper helper, IApplicantRepository applicant, IAdmissionRepository admission)
    {
        _helper = helper;
        _applicant = applicant;
        _admission = admission;
    }

    public async Task<Unit> Handle(DeleteAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions");
        
        if (!await _admission.CheckIfAdmissionExists(request.AdmissionRequestDto.AdmissionId))
            throw new BadRequest("Provided admission does not exist");
        //todo: check
        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.AdmissionRequestDto.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");

        var admission = await _admission.GetById(request.AdmissionRequestDto.AdmissionId);
        await _admission.DeleteAsync(admission);
        await _helper.RearrangeAdmissionsByDeletion(request.UserId, admission.Priority);

        return Unit.Value;
    }
}