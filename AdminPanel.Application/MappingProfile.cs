using AdminPanel.Domain.Entities;
using AutoMapper;
using Common.Models.Models.Dtos;

namespace AdminPanel.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ManagerDto, BaseManager>();
    }
}