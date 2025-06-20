using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
             var regions = await _dbContext.Regions.ToListAsync();
            return regions;
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);         
            return region;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r =>r.Id== id);
            if(existingRegion == null)
            {
                return null; // or throw an exception
            }
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;  
            await _dbContext.SaveChangesAsync();    
            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var exitingRegion = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (exitingRegion == null)
            {
                return null;
            }
            _dbContext.Regions.Remove(exitingRegion);
            await _dbContext.SaveChangesAsync();
            return exitingRegion;
        }
    }
}
