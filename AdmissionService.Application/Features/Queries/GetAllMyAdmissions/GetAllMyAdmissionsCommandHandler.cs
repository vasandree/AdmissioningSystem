using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Application.Dtos.Responses;
using AutoMapper;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetAllMyAdmissions;

public class GetAllMyAdmissionsCommandHandler : IRequestHandler<GetAllMyAdmissionsCommand, List<AdmissionDto>>
{
    private readonly IAdmissionRepository _admission;
    private readonly IMapper _mapper;

    public GetAllMyAdmissionsCommandHandler(IAdmissionRepository admission, IMapper mapper)
    {
        _admission = admission;
        _mapper = mapper;
    }

    public async Task<List<AdmissionDto>> Handle(GetAllMyAdmissionsCommand request, CancellationToken cancellationToken)
    {
        var admissions = await _admission.GetApplicantsAdmissions(request.UserId);
        return admissions.Select(admission => _mapper.Map<AdmissionDto>(admission)).ToList();
    }
}