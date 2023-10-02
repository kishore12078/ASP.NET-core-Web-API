using CityInfoAPI.Entities;

namespace CityInfoAPI.Interfaces
{
    public interface ICityInfoRepo
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityByIdAsync(int cityId,bool includePointOfInterest);
        Task<bool> CheckCityExists(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId,int pointOfInterestId);
        Task CreatePointOfInterest(int cityId, PointOfInterest pointOfInterest);
        Task<IEnumerable<City>> CityFiltering(string? name);
    }
}
