using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetPrograms;

public class GetProgramsCommandHandler : IRequestHandler<GetProgramsCommand, ProgramsPagedListDto>
{
    public Task<ProgramsPagedListDto> Handle(GetProgramsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}