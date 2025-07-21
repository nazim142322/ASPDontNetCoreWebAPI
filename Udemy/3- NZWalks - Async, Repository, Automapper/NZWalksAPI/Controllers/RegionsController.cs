using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;

namespace NZWalksAPI.Controllers
{
    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, 
            IMapper mapper, ILogger<RegionsController> logger )
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //Get All Regions
        //Get: https://localhost:7050/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            
            _logger.LogInformation("GetAll Regions API Method is called");            

            //calling repository to get all regions
            var regionsDomain = await _regionRepository.GetAllAsync();

            //_logger.LogInformation($"Finished getting all regions. Total Regions: {regionsDomain.Count()}"); 
            _logger.LogInformation($"Finished getting all regions request{JsonSerializer.Serialize(regionsDomain)}");

            //Map Domain Models to DTOs
            //var regionDTOs = new List<RegionDTO>();
            //foreach (var region in regionsDomain)
            //{
            //    var regionDTO = new RegionDTO
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    };
            //    regionDTOs.Add(regionDTO);
            //}
            //Map Domain Models to DTOs using AutoMapper
            var regionDTOs = _mapper.Map<IEnumerable<RegionDTO>>(regionsDomain);//destination to source

            //return Ok(regionsDomain);
            return Ok(regionDTOs);
        }

        //Get Region By Id
        //Get: https://localhost:7050/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Get Region Domain Model from database
            var regionDomain = await _regionRepository.GetByIdAsync(id);           
           
            if (regionDomain == null)
            {
                return NotFound();
            }
            //convert Region Domain Model to Region DTO

            //var regionDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            //Map Domain Model to DTO using AutoMapper
            var regionDTO = _mapper.Map<RegionDTO>(regionDomain);
            //return DTO back to Client
            return Ok(regionDTO);
        }

        //Post to create New Region
        //POST: https://locathost:7050/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addregiondto)
        {
            if (addregiondto == null)
            {
                return BadRequest();
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // yaha ModelState.IsValid likhne ki zarurat nahi
            var regionDomainModel = _mapper.Map<Region>(addregiondto); //destination to source

            //calling repository to create region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);            

            //Convert Domain Model to DTO        
            var regionDTO= _mapper.Map<RegionDTO>(regionDomainModel); //destination to source

            //return Ok(regionDTO);
            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        //update region
        //PUT: https://localhost:7050/api/regions/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegiondto)
        {
            if (updateRegiondto == null)
            {
                return BadRequest();
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //yaha ModelState.IsValid likhne ki zarurat nahi

            //Map DTO to Domain Model
            var RegionDomainModel =  _mapper.Map<Region>(updateRegiondto); //destination to source

            //calling repository to update region
            var newRegion = await _regionRepository.UpdateAsync(id, RegionDomainModel);
             
            if (newRegion == null)
            {
                return NotFound();
            }         
            //Convert Domain Model to DTO            
            var regionDTO =  _mapper.Map<RegionDTO>(newRegion); //destination to source
            return Ok(regionDTO);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer ,Reader")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            //calling repository to delete region
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //return the deleted Region back
            //Map Domain Model to DTO using AutoMapper
            var regiondto = _mapper.Map<RegionDTO>(regionDomainModel); //destination to source
            return Ok(regiondto);
            //return Ok(new {message="record deleted successfully"});             
        }
       
    }
}
//[FromForm] Guid id-id form-data se aayega (jo usually POST/PUT request me hota hai).
//[HttpPut("updateregion/{id:Guid}")]