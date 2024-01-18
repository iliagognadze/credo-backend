using AutoMapper;
using Entities.Models;
using Shared.DTOs;

namespace Credo.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserForCreationDto, User>();
        CreateMap<Entities.Models.Application, ApplicationDto>();
        CreateMap<ApplicationForCreationDto, Entities.Models.Application>();
        CreateMap<ApplicationForUpdateDto, Entities.Models.Application>();
    }
}