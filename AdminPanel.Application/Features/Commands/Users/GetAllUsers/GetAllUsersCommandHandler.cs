using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Models;
using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.GetAllUsers;

public class GetAllUsersCommandHandler : IRequestHandler<GetAllUsersCommand, PagedUserDto>
{
    private readonly RpcRequestSender _rpc;

    public GetAllUsersCommandHandler(RpcRequestSender rpc)
    {
        _rpc = rpc;
    }

    public async Task<PagedUserDto> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
    {
        var users = await _rpc.GetAllUsers();

        var pagedApplicants = users.Skip((request.Page - 1) * request.Size).Take(request.Page).ToList();

        int totalApplicantsCount = users.Count;
        int totalPages = (int)Math.Ceiling((double)totalApplicantsCount / request.Size);

        return new PagedUserDto
        {
            Users = pagedApplicants,
            Pagination = new Pagination(request.Size, totalPages, request.Page)
        };
    }
}