using Microsoft.EntityFrameworkCore;
using Sample.API.Entities;

namespace Sample.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData
                (new City()
                {
                    Id = 1,
                    Name = "Leeds",
                    Description = "A city in the northern English county of Yorkshire"
                },
                new City()
                {
                    Id = 2,
                    Name = "Sheffield",
                    Description = "A city in the English county of South Yorkshire"
                });


            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest() 
                { 
                    Id = 1, 
                    CityId = 1,
                    Name = "Kirkstall Abbey", 
                    Description = "A ruined Cistercian monastery" 
                },
                new PointOfInterest() 
                { 
                    Id = 2,
                    CityId = 1,
                    Name = "Kirkgate Market", 
                    Description = "One of the largest indoor markets in Europe" 
                },
                new PointOfInterest() 
                { 
                    Id = 3,
                    CityId = 2,
                    Name = "Botanical Gardens", 
                    Description = "Expansive 19th-century gardens" 
                },
                new PointOfInterest() 
                { 
                    Id = 4,
                    CityId = 2,
                    Name = "Kelham Island Museum", 
                    Description = "Museum of Sheffield's industrial history" 
                });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

    }
}
