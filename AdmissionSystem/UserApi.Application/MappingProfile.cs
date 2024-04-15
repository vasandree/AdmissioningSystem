using AutoMapper;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<EditProfileDto, ApplicationUser>();
    }
}