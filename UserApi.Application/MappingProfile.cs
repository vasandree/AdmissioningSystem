using System.Runtime.Intrinsics.X86;
using AutoMapper;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, ApplicationUser>().ForMember(x => x.UserName, o 
            => o.MapFrom(x => x.Email));
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<EditProfileDto, ApplicationUser>();
    }
}