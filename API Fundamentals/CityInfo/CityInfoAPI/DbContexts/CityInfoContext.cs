using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.DbContexts
{
    public class CityInfoContext:DbContext
    {
        public CityInfoContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=KISHORE\\SQLEXPRESS;Integrated Security=true;Initial Catalog=CityInfo");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
