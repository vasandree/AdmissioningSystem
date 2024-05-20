using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using Common.Models.Dtos.PagedDtos;
using DictionaryService.Application.Contracts.Persistence;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetFaculties;

public class GetFacultiesCommandHandler : IRequestHandler<GetFacultiesCommand, FacultiesPagedDto>
{
    private readonly IFacultyRepository _faculty;
    private readonly IMapper _mapper;

    public GetFacultiesCommandHandler(IFacultyRepository faculty, IMapper mapper)
    {
        _faculty = faculty;
        _mapper = mapper;
    }

    public async Task<FacultiesPagedDto> Handle(GetFacultiesCommand request, CancellationToken cancellationToken)
    {
        if (request.Size < 1 || request.Page < 1)
            throw new BadRequest("Size and Page must be greater than or equal to 1.");

        var faculties = await _faculty.GetAllAsync();

        var totalPages = (int)Math.Ceiling((double)faculties.Count / request.Size);

        if (totalPages < request.Page) throw new BadRequest("Invalid value for attribute page");

        var resultFaculties = faculties.Skip(request.Size * (request.Page - 1)).Take(request.Size).ToList();

        var facultyDtos = resultFaculties.Select(x => _mapper.Map<FacultyDto>(x)).ToList();
        
        return new FacultiesPagedDto
        {
            Faculties = facultyDtos,
            Pagination = new Pagination(request.Size, totalPages, request.Page)
        };
    }
}