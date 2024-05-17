using AdmissionService.Application.Dtos.Responses;
using AdmissionService.Domain.Entities;
using AutoMapper;

namespace AdmissionService.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Admission, AdmissionDto>();
    }
}