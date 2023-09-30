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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().
                HasData(
                new City("Chennai")
                { Id=1,Description="Wonderful and emerging place in india"},
                new City("Mumbai")
                { Id = 2, Description = "Potential and workable place in india" },
                new City("Delhi")
                { Id = 3, Description = "Capital of India" }
                );
            modelBuilder.Entity<PointOfInterest>().
                HasData(
                new PointOfInterest("Kishore")
                {Id=1,CityId=1, Description="Learning" },
                new PointOfInterest("Mathan")
                { Id = 2,CityId=3, Description = "Learning" },
                new PointOfInterest("Gokul")
                { Id = 3,CityId=1, Description = "Learning" }
                );
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=KISHORE\\SQLEXPRESS;Integrated Security=true;Initial Catalog=CityInfo");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
