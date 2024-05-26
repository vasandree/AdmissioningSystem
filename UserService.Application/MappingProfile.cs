using System.Runtime.Intrinsics.X86;
using AutoMapper;
using Common.Models.Models.Dtos;
using UserApi.Domain.DbEntities;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;

namespace UserService.Application;

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