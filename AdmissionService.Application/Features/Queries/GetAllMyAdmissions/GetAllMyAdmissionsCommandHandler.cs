using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Dtos.Responses;
using AdmissionService.Application.ServiceBus.RPC;
using AutoMapper;
using Common.Models.Models;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetAllMyAdmissions;

public class GetAllMyAdmissionsCommandHandler : IRequestHandler<GetAllMyAdmissionsCommand, List<AdmissionDto>>
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

    public async Task<List<AdmissionDto>> Handle(GetAllMyAdmissionsCommand request, CancellationToken cancellationToken)
    {
        var admissions = await _admission.GetApplicantsAdmissions(request.UserId);

        var resultDto = new List<AdmissionDto>();
        foreach (var admission in admissions)
        {
            var dto = _mapper.Map<AdmissionDto>(admission);
            var program = await _rpc.GetProgram(admission.ProgramId);
            
            resultDto.Add(dto);
        }

        return resultDto;
    }
}