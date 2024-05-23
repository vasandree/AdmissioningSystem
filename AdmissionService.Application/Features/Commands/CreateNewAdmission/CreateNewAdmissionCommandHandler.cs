using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using AdmissionService.Application.RPC;
using AdmissionService.Domain.Entities;
using AdmissionService.Domain.Enums;
using Common.Models.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Commands.CreateNewAdmission;

public class CreateNewAdmissionCommandHandler : IRequestHandler<CreateNewAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;
    private readonly RpcRequestsSender _rpc;

    public CreateNewAdmissionCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        AdmissionsRearrangeHelper helper, RpcRequestsSender rpc)
    {
        _admission = admission;
        _applicant = applicant;
        _helper = helper;
        _rpc = rpc;
    }

    public async Task<Unit> Handle(CreateNewAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (!await _admission.CheckIfAdmissionIsAvailable(request.UserId))
            throw new BadRequest("You added the maximum number of programs");

        var educationDocId = await _rpc.CheckIfApplicantHasDocument(request.UserId);

        if (educationDocId == null)
            throw new BadRequest("You have to add an education document first");

        //todo: check
        
        var educationDoc = await _rpc.GetEducationDocument(educationDocId.Value);

        if (educationDoc == null)
            throw new BadRequest("Failed to retrieve the education document");


        var program = await _rpc.GetProgram(request.CreateAdmissionRequest.ProgramId);

        if (program == null)
            throw new BadRequest("Provided program does not exist");

        if (!_admission.CheckIfEducationLevelIsAvailable(program, educationDoc))
            throw new BadRequest("Education Level of this program is not available for you");

        if (!await _admission.CheckIfEducationStageIsAvailable(request.UserId, program, educationDoc))
            throw new BadRequest("Education Level of this program is not available for you." +
                                 "Because you chose previous level of education in other admissions");

        if (!await _admission.CheckIfPriorityAvailable(request.UserId, request.CreateAdmissionRequest.Priority))
        {
            await _helper.RearrangeAdmissionsByAddingNewOne(request.UserId, request.CreateAdmissionRequest.Priority);
        }


        Applicant applicant;
        if (await _applicant.CheckIfApplicantExists(request.UserId))
        {
            applicant = await _applicant.GetById(request.UserId);
        }
        else
        {
            applicant = new Applicant
            {
                ApplicantId = request.UserId,
                EducationDocumentId = educationDoc.Id
            };

            await _applicant.CreateAsync(applicant);
        }


        await _admission.CreateAsync(new Admission
        {
            AdmissionId = Guid.NewGuid(),
            ApplicantId = request.UserId,
            Status = AdmissionStatus.Created,
            Priority = request.CreateAdmissionRequest.Priority,
            ProgramId = request.CreateAdmissionRequest.ProgramId,
            Applicant = applicant,
            EducationLevelId = program.EducationLevel.Id,
            IsDeleted = false,
            ManagerId = null
        });

        //todo: add role "Applicant"

        return Unit.Value;
    }
}