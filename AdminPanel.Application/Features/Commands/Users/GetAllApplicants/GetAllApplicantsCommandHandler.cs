using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Models;
using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.GetAllApplicants;

public class GetAllApplicantsCommandHandler : IRequestHandler<GetAllApplicantsCommand, PagedUserDto>
{
    private readonly RpcRequestSender _rpc;

    public GetAllApplicantsCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<PagedUserDto> Handle(GetAllApplicantsCommand request, CancellationToken cancellationToken)
    {
        var applicants =await _rpc.GetAllApplicants();
        
        var pagedApplicants = applicants.Skip((request.Page - 1) * request.Size).Take(request.Page).ToList();
        
        int totalApplicantsCount = applicants.Count;
        int totalPages = (int)Math.Ceiling((double)totalApplicantsCount / request.Size);

        return new PagedUserDto
        {
            Users = pagedApplicants,
            Pagination = new Pagination(request.Size, totalPages, request.Page)
        };

    }
}