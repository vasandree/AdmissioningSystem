using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Domain.Entities;
using AdmissionService.Domain.Enums;
using Common.Exceptions;
using Common.Models.Dtos;
using MediatR;

namespace AdmissionService.Application.Features.Commands.CreateNewAdmission;

public class CreateNewAdmissionCommandHandler : IRequestHandler<CreateNewAdmissionCommand, Unit>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;

    public CreateNewAdmissionCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant)
    {
        _admission = admission;
        _applicant = applicant;
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
            Priority = request.AdmissionRequest.Priority,
            Program = program,
            Applicant = applicant
        });

        return Unit.Value;
    }
}