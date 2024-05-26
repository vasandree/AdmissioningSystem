using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using AdmissionService.Application.ServiceBus.PubSub.Senders;
using AdmissionService.Application.ServiceBus.RPC;
using AdmissionService.Domain.Entities;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdmissionService.Application.Features.Commands.CreateNewAdmission;

public class CreateNewAdmissionCommandHandler : IRequestHandler<CreateNewAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;
    private readonly RpcRequestsSender _rpc;
    private readonly PubSubSender _pubSub;

    public CreateNewAdmissionCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        AdmissionsRearrangeHelper helper, RpcRequestsSender rpc, PubSubSender pubSub)
    {
        _admission = admission;
        _applicant = applicant;
        _helper = helper;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(CreateNewAdmissionCommand request, CancellationToken cancellationToken)
    {
        if (await _admission.CheckClosed(request.UserId))
            throw new BadRequest("You cannot edit info, because your admission is closed");

        if (!await _admission.CheckIfAdmissionIsAvailable(request.UserId))
            throw new BadRequest("You added the maximum number of programs");

        if (_admission.CheckIfProgramIsChosen(request.UserId, request.CreateAdmissionRequest.ProgramId))
            throw new BadRequest("You have already created an admission for this program");

        var educationDocId = await _rpc.CheckIfApplicantHasDocument(request.UserId);

        if (educationDocId == null)
            throw new BadRequest("You have to add an education document first");

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
        if (await _applicant.CheckIfApplicantExists(request.UserId))
        {
            if (!_admission.CheckIfNewPriorityIsAvailable(request.UserId, request.CreateAdmissionRequest.Priority))
                throw new BadRequest("New priority is out of range of applicant's admissions");
        }

        if (await _admission.CheckIfPriorityAvailable(request.UserId, request.CreateAdmissionRequest.Priority) == false)
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

            await _pubSub.UpdateApplicantRole(applicant.ApplicantId);
            await _applicant.CreateAsync(applicant);
        }

        var admission = new Admission
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
            };
        
        await _admission.CreateAsync(admission);

        await _pubSub.Admission(admission.AdmissionId);

    return Unit.Value;
}

}