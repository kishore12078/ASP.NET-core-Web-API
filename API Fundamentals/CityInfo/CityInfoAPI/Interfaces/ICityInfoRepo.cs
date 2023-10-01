using CityInfoAPI.Entities;

namespace CityInfoAPI.Interfaces
{
    public interface ICityInfoRepo
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityByIdAsync(int cityId,bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int pointOfInterestId);
    }
}
