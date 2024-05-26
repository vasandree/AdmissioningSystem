using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.Rpc;
using AutoMapper;
using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Admissions.GetAllAdmissions;

public class GetAllAdmissionsQueryHandler : IRequestHandler<GetAllAdmissionsQuery, Unit>
{
    private readonly RpcRequestSender _rpc;
    private readonly IMapper _mapper;
    private readonly IAdmissionRepository _admission;
    private readonly IBaseManagerRepository _baseManager;
    private readonly IManagerRepository _manager;

    public GetAllAdmissionsQueryHandler(RpcRequestSender rpc, IAdmissionRepository repository, IBaseManagerRepository manager, IMapper mapper, IManagerRepository manager1)
    {
        _rpc = rpc;
        _admission = repository;
        _baseManager = manager;
        _mapper = mapper;
        _manager = manager1;
    }

    public async Task<Unit> Handle(GetAllAdmissionsQuery request, CancellationToken cancellationToken)
    {
        var admissions = await _rpc.GetAdmissions(request);
        foreach (var admission in admissions.Admissions.Admissions)
        {
            var realAdmission = await _admission.GetById(admission.Id);
            if (realAdmission.ManagerId != null)
            {
                var manager = await _baseManager.GetById(realAdmission.ManagerId);
                var dto = _mapper.Map<ManagerDto>(manager);
                if (await _baseManager.CheckIfManager(manager))
                {
                    dto.FacultyId = await _manager.GetFaculty(manager.Id);
                }
            }
        }
        return Unit.Value;
    }
}