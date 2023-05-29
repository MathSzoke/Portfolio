using AutoMapper;
using Backend.Data.UserRegistrationData.DTO;
using Backend.Models;

namespace Backend.Data.UserRegistrationData.Mapper;

public class MapperUserRegistration : Profile
{
    public MapperUserRegistration() 
    {
        CreateMap<PostUserRegistrationDTO, UserRegistration>();
        CreateMap<UserRegistration, PostUserRegistrationDTO>();
        CreateMap<GetUserRegistrationDTO, UserRegistration>();
        CreateMap<UserRegistration, GetUserRegistrationDTO>();
    }
}
