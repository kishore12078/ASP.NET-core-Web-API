using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.DbContexts
{
    public class CityInfoContext:DbContext
    {
        public DbSet<City> cities { get; set; } = null!;
        public DbSet<PointOfInterest> pointOfInterests { get; set; } = null!;
    }
}
