using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Dtos.Responses;
using AdmissionService.Application.RPC;
using AutoMapper;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetAllMyAdmissions;

public class GetAllMyAdmissionsCommandHandler : IRequestHandler<GetAllMyAdmissionsCommand, List<AdmissionListDto>>
{
    private readonly IAdmissionRepository _admission;
    private readonly IMapper _mapper;
    private readonly RpcRequestsSender _rpc;

    public GetAllMyAdmissionsCommandHandler(IAdmissionRepository admission, IMapper mapper, RpcRequestsSender rpc)
    {
        _admission = admission;
        _mapper = mapper;
        _rpc = rpc;
    }

    public async Task<List<AdmissionListDto>> Handle(GetAllMyAdmissionsCommand request, CancellationToken cancellationToken)
    {//todo: check
        var admissions = await _admission.GetApplicantsAdmissions(request.UserId);

        var resultDto = new List<AdmissionListDto>();
        foreach (var admission in admissions)
        {
            var dto = _mapper.Map<AdmissionListDto>(admission);
            var program = await _rpc.GetProgram(admission.ProgramId);
            dto.ProgramName = program!.Name;
            
            resultDto.Add(dto);
        }

        return resultDto;
    }
}