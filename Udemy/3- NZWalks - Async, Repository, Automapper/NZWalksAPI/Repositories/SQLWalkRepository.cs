using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using System.Reflection.Metadata.Ecma335;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return (walk);//returning with id
        }

       public async Task<IEnumerable<Walk>> GetAllWalksAsync()
        {                   
            //var walks = await _dbContext.Walks
                //.Include(w => w.Region) // Include the Region navigation property
                //.Include(w => w.Difficulty) // Include the Difficulty navigation property
                //.ToListAsync();
            //return walks;
            var walks2 = await _dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
            return walks2;
        }

       public Task<Walk> GetByIdAsync(Guid id)
        {
            var walk = _dbContext.Walks.Include(w=> w.Region) // Include the Region navigation property
                .Include(w => w.Difficulty) // Include the Difficulty navigation property
                .FirstOrDefaultAsync(w => w.Id == id);
            return walk;
        }

       public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk = await _dbContext.Walks.FindAsync(id);
            if (existingWalk == null)
            {
                return null; // Walk not found
            }
            // Update the properties of the existing walk                    
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            await _dbContext.SaveChangesAsync();
            return existingWalk; // Return the updated walk
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var exitingWalk = await _dbContext.Walks.FindAsync(id);
            if (exitingWalk == null)
            {
                return null; // Walk not found
            }
            _dbContext.Walks.Remove(exitingWalk);
            await _dbContext.SaveChangesAsync();
            return exitingWalk; // Return the deleted walk
        }
    }
}
