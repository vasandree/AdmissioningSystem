using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Dtos.Responses;
using AutoMapper;
using Common.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetMyAdmissionById;

public class GetAdmissionByIdCommandHandler : IRequestHandler<GetAdmissionByIdCommand, AdmissionDto>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly IMapper _mapper;

    public GetAdmissionByIdCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        IMapper mapper)
    {
        _admission = admission;
        _applicant = applicant;
        _mapper = mapper;
    }

    public async Task<AdmissionDto> Handle(GetAdmissionByIdCommand request, CancellationToken cancellationToken)
    {
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions yet");

        if (!await _admission.CheckIfAdmissionExists(request.AdmissionRequestDto.AdmissionId))
            throw new BadRequest("Provided admission does not exist");

        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.AdmissionRequestDto.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");

        var admission = _admission.GetById(request.AdmissionRequestDto.AdmissionId);
        return _mapper.Map<AdmissionDto>(admission);
    }
}