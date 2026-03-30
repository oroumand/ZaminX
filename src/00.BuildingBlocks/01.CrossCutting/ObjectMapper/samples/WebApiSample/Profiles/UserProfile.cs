using AutoMapper;
using ZaminX.Mapper.WebApiSample.Models;

namespace ZaminX.Mapper.WebApiSample.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>();
    }
}