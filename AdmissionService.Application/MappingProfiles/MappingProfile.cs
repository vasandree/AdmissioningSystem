using AdmissionService.Application.Dtos.Responses;
using AdmissionService.Domain.Entities;
using AutoMapper;
using Common.Models.Models;

namespace AdmissionService.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Admission, AdmissionDto>();
        
        CreateMap<Admission, Common.Models.Models.Dtos.AdmissionDto>();
    }
}