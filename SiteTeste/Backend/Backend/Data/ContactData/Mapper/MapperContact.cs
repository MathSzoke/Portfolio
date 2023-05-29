using AutoMapper;
using Backend.Data.ContactData.DTO;
using Backend.Models;

namespace Backend.Data.ContactData.Mapper;

public class MapperContact : Profile
{
    public MapperContact()
    {
        CreateMap<PostContactDTO, Contact>();
        CreateMap<UpdateContactDTO, Contact>();
        CreateMap<Contact, PostContactDTO>();
        CreateMap<Contact, UpdateContactDTO>();
    }
}
