using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository )
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
        }

        //Get All Regions
        //Get: https://localhost:7050/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
          var regionsDomain = await _regionRepository.GetAllAsync();          

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
            var regionDomain = await _regionRepository.GetByIdAsync(id);           
           
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
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);            

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
            var RegionDomainModel = new Region
            {
                Name = updateRegiondto.Name,
                Code = updateRegiondto.Code,
                RegionImageUrl = updateRegiondto.RegionImageUrl
            };

            //calling repository to update region
            var newRegion = await _regionRepository.UpdateAsync(id, RegionDomainModel);
             
            if (newRegion == null)
            {
                return NotFound();
            }          

            //Convert Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = newRegion.Id,
                Code = newRegion.Code,
                Name = newRegion.Name,
                RegionImageUrl = newRegion.RegionImageUrl
            };
            return Ok(regionDTO);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            //calling repository to delete region
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

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