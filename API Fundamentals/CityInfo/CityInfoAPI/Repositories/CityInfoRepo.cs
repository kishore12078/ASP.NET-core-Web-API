﻿using CityInfoAPI.DbContexts;
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

        public async Task<PointOfInterest?> GetPointOfInterestAsync(int pointOfInterestId)
        {
            return await _context.PointOfInterests.FirstOrDefaultAsync(p => p.Id == pointOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsAsync(int cityId)
        {
            return await _context.PointOfInterests.Where(p=>p.CityId==cityId).ToListAsync();
        }
    }
}