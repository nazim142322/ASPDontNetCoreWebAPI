using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<IEnumerable<Walk>> GetAllWalksAsync();

        //for filter
        Task<IEnumerable<Walk>> GetWalksByFilterAsync(string? filterOn = null, string? filterQuery = null);

        //for filter and sorting
        Task<IEnumerable<Walk>> GetWalksByFilterSortingAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true);

        //for filter sorting and pagination
        Task<IEnumerable<Walk>> GetWalksByFilterSortingPaginationAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true);

        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
