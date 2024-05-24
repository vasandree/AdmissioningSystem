using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Dtos.Responses;
using AdmissionService.Application.RPC;
using AutoMapper;
using Common.Models.Exceptions;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetMyAdmissionById;

public class GetAdmissionByIdCommandHandler : IRequestHandler<GetAdmissionByIdCommand, AdmissionDto>
{
    private readonly IAdmissionRepository _admission;
    private readonly IApplicantRepository _applicant;
    private readonly IMapper _mapper;
    private readonly RpcRequestsSender _rpc;

    public GetAdmissionByIdCommandHandler(IAdmissionRepository admission, IApplicantRepository applicant,
        IMapper mapper, RpcRequestsSender rpc)
    {
        _admission = admission;
        _applicant = applicant;
        _mapper = mapper;
        _rpc = rpc;
    }

    public async Task<AdmissionDto> Handle(GetAdmissionByIdCommand request, CancellationToken cancellationToken)
    {//todo: check
        if (!await _applicant.CheckIfApplicantExists(request.UserId))
            throw new BadRequest("Applicant does not have any admissions yet");

        if (!await _admission.CheckIfAdmissionExists(request.AdmissionId))
            throw new BadRequest("Provided admission does not exist");

        if (!await _admission.CheckIfAdmissionBelongsToApplicant(request.UserId,
                request.AdmissionId))
            throw new BadRequest("Applicant does not have provided admission");

        var admission = await _admission.GetById(request.AdmissionId);
        if (admission.IsDeleted) throw new NotFound("Provided admission was  deleted");
        
        var dto = _mapper.Map<AdmissionDto>(admission);

        dto.Program = await _rpc.GetProgram(admission.ProgramId!);
        
        return dto;
    }
}