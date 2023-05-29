using AutoMapper;
using Backend.Data.UserLoginData.DTO;
using Backend.Models;

namespace Backend.Data.UserLoginData.Mapper; 
public class MapperUserLogin : Profile
{
    public MapperUserLogin() 
    {
        CreateMap<PostUserLoginDTO, UserLogin>();
        CreateMap<UserLogin, PostUserLoginDTO>();
        CreateMap<GetUserLoginDTO, UserLogin>();
        CreateMap<UserLogin, GetUserLoginDTO>();
    }
}
