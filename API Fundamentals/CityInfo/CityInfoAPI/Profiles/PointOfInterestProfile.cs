using AutoMapper;

namespace CityInfoAPI.Profiles
{
    public class PointOfInterestProfile:Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Entities.PointsOfInterestDTO>();
        }
    }
}
