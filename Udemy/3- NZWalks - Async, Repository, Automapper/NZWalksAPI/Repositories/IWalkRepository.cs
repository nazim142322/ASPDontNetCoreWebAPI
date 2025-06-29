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
        Task<IEnumerable<Walk>> GetAllByFilterSortingPagination(string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending, int pageNumber = 1, int pageSize = 100);

        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
