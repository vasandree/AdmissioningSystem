using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Helpers;
using AdmissionService.Domain.Entities;
using AdmissionService.Domain.Enums;
using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using MediatR;

namespace AdmissionService.Application.Features.Commands.CreateNewAdmission;

public class CreateNewAdmissionCommandHandler : IRequestHandler<CreateNewAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly AdmissionsRearrangeHelper _helper;

    public CreateNewAdmissionCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        AdmissionsRearrangeHelper helper)
    {
        _admission = admission;
        _applicant = applicant;
        _helper = helper;
    }

    public async Task<Unit> Handle(CreateNewAdmissionCommand request, CancellationToken cancellationToken)
    {
        //todo: check if user exists
        if (!await _admission.CheckIfAdmissionIsAvailable(request.UserId))
            throw new BadRequest("You added maximum amount of programs");

        //todo: check if program exists
        //todo: get program
        //todo: get educationDocument
        //todo: check if program available

        ProgramDto program = null;
        EducationDocumentTypeDto educationDocumentType = null;
        if (!_admission.CheckIfEducationLevelIsAvailable(program, educationDocumentType))
            throw new BadRequest("Education Level of this program is not available for you");

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
                EducationDocument = educationDocumentType
            };

            await _applicant.CreateAsync(applicant);
        }


        await _admission.CreateAsync(new Admission
        {
            AdmissionId = Guid.NewGuid(),
            ApplicantId = request.UserId,
            Status = AdmissionStatus.Created,
            Priority = request.CreateAdmissionRequest.Priority,
            Program = program,
            Applicant = applicant
        });

        //todo: add role "Applicant"

        return Unit.Value;
    }
}