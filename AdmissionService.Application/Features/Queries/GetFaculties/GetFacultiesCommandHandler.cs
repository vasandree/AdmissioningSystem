using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetFaculties;

public class GetFacultiesCommandHandler : IRequestHandler<GetFacultiesCommand, FacultiesPagedDto>
{
    public Task<FacultiesPagedDto> Handle(GetFacultiesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}