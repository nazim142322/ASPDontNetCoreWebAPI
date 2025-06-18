using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

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
        //Get: https://localhost:7050/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data From Database - Domain models
            //var regionsDomain = new List<Region>
            //{
            //    new Region { Id = Guid.NewGuid(), Code = "NI", Name = "North Island", RegionImageUrl = "https://example.com/ni.jpg" },
            //    new Region { Id = Guid.NewGuid(), Code = "SI", Name = "South Island", RegionImageUrl = "https://example.com/si.jpg" }
            //};
            var regionsDomain = await _dbContext.Regions.ToListAsync();

            //Map Domain Models to DTOs
            var regionDTOs = new List<RegionDTO>();
            foreach (var region in regionsDomain)
            {
                var regionDTO = new RegionDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                };
                regionDTOs.Add(regionDTO);
            }
            //return Ok(regionsDomain);
            return Ok(regionDTOs);
        }

        //Get Region By Id
        //Get: https://localhost:7050/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //Get Region Domain Model from database
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            //var result=_dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            //convert Region Domain Model to Region DTO

            var regionDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            //return DTO back to Client
            return Ok(regionDTO);
        }

        //Post to create New Region
        //POST: https://locathost:7050/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addregiondto)
        {
            if (addregiondto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Name = addregiondto.Name,
                Code = addregiondto.Code,
                RegionImageUrl = addregiondto.RegionImageUrl
            };
            await _dbContext.Regions.AddAsync(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            //Convert Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            //return Ok(regionDTO);
            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        //update region
        //PUT: https://localhost:7050/api/regions/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegiondto)
        {
            if (updateRegiondto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get Region Domain Model from database
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Update Domain Model
            regionDomain.Name = updateRegiondto.Name;
            regionDomain.Code = updateRegiondto.Code;
            regionDomain.RegionImageUrl = updateRegiondto.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            //Convert Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDTO);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
           var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            _dbContext.Remove(regionDomainModel);
            await _dbContext.SaveChangesAsync();

            //return the deleted Region back
            var regiondto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regiondto);
            //return Ok(new {message="record deleted successfully"});             
        }
       
    }
}
//[FromForm] Guid id-id form-data se aayega (jo usually POST/PUT request me hota hai).
//[HttpPut("updateregion/{id:Guid}")]