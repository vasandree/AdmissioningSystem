using Common.Models.Exceptions;
using MediatR;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Queries.RolesCommands.GetUserRoles;

public class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, RolesDto>
{
    private readonly IUserRepository _repository;

    public GetUserRolesHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<RolesDto> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(request.Id);
        if (user == null) throw new BadRequest("No such user");
        
        var roles = await _repository.GetUserRoles(user);
        return InsertRolesIntoDto(roles);
    }

    private RolesDto InsertRolesIntoDto(IList<string> roles)
    {
        var dto = new RolesDto();
        foreach (var role in roles)
        {
            switch (role)
            {
                case "Admin":
                    dto.IsAdmin = true;
                    break;
                case "Applicant" :
                    dto.IsApplicant = true;
                    break;
                case "Manager":
                    dto.IsHeadManager = true;
                    break;
                case "HeadManager":
                    dto.IsHeadManager = true;
                    break;
            }
        }

        return dto;
    }
}