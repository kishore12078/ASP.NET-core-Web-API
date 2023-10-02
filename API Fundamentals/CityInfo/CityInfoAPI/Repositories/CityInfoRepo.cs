using CityInfoAPI.DbContexts;
using CityInfoAPI.Entities;
using CityInfoAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.Repositories
{
    public class CityInfoRepo:ICityInfoRepo
    {
        private readonly CityInfoContext _context;

        public CityInfoRepo(CityInfoContext context)
        {
            _context=context;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c=>c.Name).ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int cityId,bool includePointOfInterest)
        {
            if (includePointOfInterest)
                return await _context.Cities.Include(p => p.PointsOfInterests).FirstOrDefaultAsync(c=>c.Id==cityId);
            return await _context.Cities.FirstOrDefaultAsync(c=>c.Id==cityId);
        }

        public async Task<bool> CheckCityExists(int cityId)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c=>c.Id==cityId);
            if (city != null)
                return true;
            return false;
        }

        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId,int pointOfInterestId)
        {
            return await _context.PointOfInterests.FirstOrDefaultAsync(p =>p.CityId==cityId && p.Id == pointOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsAsync(int cityId)
        {
            return await _context.PointOfInterests.Where(p=>p.CityId==cityId).ToListAsync();
        }

        public async Task CreatePointOfInterest(int cityId,PointOfInterest pointOfInterest)
        {
            var city = await GetCityByIdAsync(cityId,true);
            if (city != null)
            {
                city.PointsOfInterests.Add(pointOfInterest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityByIdAsync(cityId,true);
            PointOfInterest? newPointOfInterest;
            if (city != null)
            {
                newPointOfInterest = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId &&
                                                                                  p.CityId == cityId);
                if (newPointOfInterest != null)
                {
                    newPointOfInterest.Name = pointOfInterest.Name;
                    newPointOfInterest.Description = pointOfInterest.Description;
                }
            }
            
        }

        //Filtering and Searching
        public async Task<IEnumerable<City>> CityFiltering(string? name,string? queryName, int pageSize, int pageNumber)
        {
            var collection =  _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name= name.Trim().ToLower();
                collection = collection.Where(c => c.Name.ToLower() == name);
            }

            if (!string.IsNullOrWhiteSpace(queryName))
            {
                queryName = queryName.Trim().ToLower();
                collection = collection.Where(c => c.Name.Contains(queryName)
                                                        || c.Description != null && c.Description.Contains(queryName));
            }
            return await collection.OrderBy(c=>c.Name)
                                   .Skip(pageSize*(pageNumber-1))
                                   .Take(pageSize)
                                   .ToListAsync();
        }
    }
}
