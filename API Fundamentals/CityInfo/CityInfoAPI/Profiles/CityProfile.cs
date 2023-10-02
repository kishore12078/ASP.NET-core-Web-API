using AutoMapper;

namespace CityInfoAPI.Profiles
{
    public class CityProfile:Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CitiesWithoutPointOfInterestsDTO>();
            CreateMap<Entities.City,Entities.CityDTO>();
        }
    }
}
