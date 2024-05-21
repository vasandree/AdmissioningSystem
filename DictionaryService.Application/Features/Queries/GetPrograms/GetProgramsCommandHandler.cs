using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Common.Models.Dtos;
using Common.Models.Dtos.PagedDtos;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Dictionaries;
using DictionaryService.Domain.Entities;
using DictionaryService.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Application.Features.Queries.GetPrograms;

public class GetProgramsCommandHandler : IRequestHandler<GetProgramsCommand, ProgramsPagedListDto>
{
    private readonly IProgramRepository _program;
    private readonly IFacultyRepository _faculty;
    private readonly IMapper _mapper;
    private readonly EducationFormDictionary _educationFormDictionary;
    private readonly LanguageDictionary _languageDictionary;

    public GetProgramsCommandHandler(IProgramRepository program, IFacultyRepository faculty, IMapper mapper, LanguageDictionary languageDictionary, EducationFormDictionary educationFormDictionary)
    {
        _program = program;
        _faculty = faculty;
        _mapper = mapper;
        _languageDictionary = languageDictionary;
        _educationFormDictionary = educationFormDictionary;
    }

    public async Task<ProgramsPagedListDto> Handle(GetProgramsCommand request, CancellationToken cancellationToken)
    {
        var programs = _program.GetAllAsQueryable();
        if (request.Faculties?.Length > 0)
        {
            foreach (var facultyId in request.Faculties)
            {
                if (!await _faculty.CheckIfExists(facultyId))
                    throw new NotFound($"Faculty with id = {facultyId} does not exist");
            }

            programs = GetFaculties(programs, request.Faculties);
        }

        programs = GetLanguage(programs, request.Language);
        programs = GetEducationForm(programs, request.FormOfEducation);
        programs = GetProgramCode(programs, request.Code);
        programs = GetProgramName(programs, request.Name);
        
        var totalProgramsCount = await programs.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalProgramsCount / request.Size);
        if (totalPages < request.Page)
            throw new BadRequest("Invalid value for attribute page");

        var resultPrograms = await programs.Skip(request.Size * (request.Page - 1))
            .Take(request.Size)
            .ToListAsync();

        var programDtos = resultPrograms.Select(x => _mapper.Map<ProgramDto>(x)).ToList();

        return new ProgramsPagedListDto
        {
            Programs = programDtos,
            Pagination = new Pagination(request.Size,  totalPages, request.Page)
        };
    }

    private IQueryable<Program> GetProgramName(IQueryable<Program> programs, string? requestName)
    {
        return !string.IsNullOrEmpty(requestName) ? programs.Where(x=>x.Name.Contains(requestName)) : programs;
    }

    private IQueryable<Program> GetProgramCode(IQueryable<Program> programs, string? code)
    {
        return !string.IsNullOrEmpty(code) ? programs.Where(x=>x.Code.Contains(code)) : programs;
    }

    private IQueryable<Program> GetLanguage(IQueryable<Program> programs, Language? language)
    {
        return language != null
            ? programs.Where(x => x.Language == _languageDictionary.GetLanguage(language.Value))
            : programs;
    }

    private IQueryable<Program> GetEducationForm(IQueryable<Program> programs, FormOfEducation? formOfEducation)
    {
        return formOfEducation != null
            ? programs.Where(x => x.EducationForm == _educationFormDictionary.GetFormOfEducation(formOfEducation.Value))
            : programs;
    }

    private static IQueryable<Program> GetFaculties(IQueryable<Program> programs, Guid[] facultyIds)
    {
        return programs.Where(p => facultyIds.Contains(p.Faculty.Id));
    }
}