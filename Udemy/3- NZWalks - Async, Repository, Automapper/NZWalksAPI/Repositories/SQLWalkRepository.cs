using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using System.Globalization;
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

     
       // This method allows filtering on properties of the Walks
       public async Task<IEnumerable<Walk>> GetWalksByFilterAsync( string? filterOn=null, string? filterQuery=null)
        {     
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();
            if(!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {  
                //single field search
                //if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                //{
                    //walks = walks.Where(w => w.Name.Contains(filterQuery));
                //}

                //mutiple fields support
                switch (filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(w => w.Name.Contains(filterQuery));
                        break;
                    case "description":
                        walks = walks.Where(w => w.Description.Contains(filterQuery));
                        break;
                    case "region":
                        walks = walks.Where(w => w.Region.Name.Contains(filterQuery));
                        break;
                    case "difficulty":
                        walks = walks.Where(w => w.Difficulty.Name.Contains(filterQuery));
                        break;
                }
            }
            var result =  await walks.ToListAsync();
            return result;
        }

        // filter and sorting
        public async Task<IEnumerable<Walk>> GetWalksByFilterSortingAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy=null, bool isAscending = true )
        {
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                //single field search
                //if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                //{
                //walks = walks.Where(w => w.Name.Contains(filterQuery));
                //}

                //mutiple fields support
                switch (filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(w => w.Name.Contains(filterQuery));
                        break;
                    case "description":
                        walks = walks.Where(w => w.Description.Contains(filterQuery));
                        break;
                    case "region":
                        walks = walks.Where(w => w.Region.Name.Contains(filterQuery));
                        break;
                    case "difficulty":
                        walks = walks.Where(w => w.Difficulty.Name.Contains(filterQuery));
                        break;
                }                
            }
            //sorting by Name length 
            //if(!string.IsNullOrWhiteSpace(sortBy))
            //{
            //    if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            //    {
            //         walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
            //    }
            //    else if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
            //    {
            //       walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
            //    }                    
            //}
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                        break;
                    case "length":
                        walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                        break;
                }
            }
            var result = await walks.ToListAsync();
            return result;
        }

        // filter sorting and pagination
        public async Task<IEnumerable<Walk>> GetWalksByFilterSortingPaginationAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true)
        {
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                //single field search
                //if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                //{
                //walks = walks.Where(w => w.Name.Contains(filterQuery));
                //}

                //mutiple fields support
                switch (filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(w => w.Name.Contains(filterQuery));
                        break;
                    case "description":
                        walks = walks.Where(w => w.Description.Contains(filterQuery));
                        break;
                    case "region":
                        walks = walks.Where(w => w.Region.Name.Contains(filterQuery));
                        break;
                    case "difficulty":
                        walks = walks.Where(w => w.Difficulty.Name.Contains(filterQuery));
                        break;
                }
            }
            //sorting by Name length 
            //if(!string.IsNullOrWhiteSpace(sortBy))
            //{
            //    if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            //    {
            //         walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
            //    }
            //    else if (sortBy.Equals("length", StringComparison.OrdinalIgnoreCase))
            //    {
            //       walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
            //    }                    
            //}
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                        break;
                    case "length":
                        walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                        break;
                }
            }
            var result = await walks.ToListAsync();
            return result;
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
