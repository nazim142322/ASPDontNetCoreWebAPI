using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Controllers
{
    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDbContext _dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get All Regions
        //https://localhost:7050/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region { Id = Guid.NewGuid(), Code = "NI", Name = "North Island", RegionImageUrl = "https://example.com/ni.jpg" },
            //    new Region { Id = Guid.NewGuid(), Code = "SI", Name = "South Island", RegionImageUrl = "https://example.com/si.jpg" }
            //};
            var regions = _dbContext.Regions.ToList();
            return Ok(regions);
        }
    }
}
