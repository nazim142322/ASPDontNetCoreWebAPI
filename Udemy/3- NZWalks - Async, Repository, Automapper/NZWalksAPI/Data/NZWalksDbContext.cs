using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Controllers;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //seed data for Difficulties
            var difficulties = new List<Difficulty>()
            {
                new Difficulty(){Id = Guid.NewGuid(),Name = "Easy"},
                new Difficulty(){Id = Guid.NewGuid(),Name = "Medium"},
                new Difficulty(){Id = Guid.NewGuid(),Name = "Hard"}
            };

            //seed data to the Difficulties table
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //seed data for Regions
            var regions = new List<Region>()
            {
                new Region {Id = Guid.NewGuid(), Name = "Northland", Code = "NTH", RegionImageUrl="https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg"},
                new Region {Id = Guid.NewGuid(), Name = "Auckland", Code = "AKL", RegionImageUrl="https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg"},
                new Region {Id = Guid.NewGuid(), Name = "Waikato", Code = "WKO", RegionImageUrl="https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg"},
                new Region {Id = Guid.NewGuid(), Name = "Bay of Plenty", Code = "BOP", RegionImageUrl="https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg"},
                new Region {Id = Guid.NewGuid(), Name = "Gisborne", Code = "GIS", RegionImageUrl="https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg"},
            };

            //seed data to the Regions table
            modelBuilder.Entity<Region>().HasData(regions);

        }
    }
    
}
