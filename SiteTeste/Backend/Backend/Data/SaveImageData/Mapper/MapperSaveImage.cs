using AutoMapper;
using Backend.Data.SaveImageData.DTO;
using Backend.Models;

namespace Backend.Data.SaveImageData.Mapper;

public class MapperSaveImage : Profile
{
	public MapperSaveImage()
    {
        CreateMap<PostSaveImageDTO, SavePhoto>();
        CreateMap<UpdateSaveImageDTO, SavePhoto>();
        CreateMap<SavePhoto, PostSaveImageDTO>();
        CreateMap<SavePhoto, UpdateSaveImageDTO>();
    }
}
