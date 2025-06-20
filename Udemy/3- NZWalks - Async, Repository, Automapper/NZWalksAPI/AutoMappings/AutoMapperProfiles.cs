using AutoMapper;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using System.Runtime;

namespace NZWalksAPI.AutoMappings
{
    public class AutoMapperProfiles:Profile
    {
       public AutoMapperProfiles()
        {
            //Create Map for Region
            CreateMap<Region, RegionDTO>().ReverseMap(); // source to destination and vice versa            
            
            CreateMap<Region, AddRegionRequestDTO>().ReverseMap(); // source to destination and vice versa

            CreateMap<Region, UpdateRegionRequestDTO>().ReverseMap(); // source to destination and vice versa

            CreateMap<Walk, AddWalkRequestDTO>().ReverseMap(); // source to destination and vice versa

            CreateMap<Walk, WalkDTO>().ReverseMap(); // source to destination and vice versa

        }
    }   
}
