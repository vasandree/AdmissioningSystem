using AutoMapper;
using Common.Models.Models.Dtos;
using DocumentService.Domain.Entities;

namespace DocumentService.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Passport, PassportInfoDto>();
        CreateMap<EducationDocument, EducationDocumentDto>();
    }
}