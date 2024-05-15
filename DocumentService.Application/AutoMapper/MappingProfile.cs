using AutoMapper;
using DocumentService.Application.Dtos.Responses;
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