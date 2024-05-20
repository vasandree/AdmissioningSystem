using AutoMapper;
using Common.Models.Dtos;
using DictionaryService.Domain.Entities;

namespace DictionaryService.Application.MappingProfiles;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DocumentType, EducationDocumentTypeDto>();
        CreateMap<EducationLevel, EducationLevelDto>();
        CreateMap<Faculty, FacultyDto>();
        CreateMap<Program, ProgramDto>();
    }
}