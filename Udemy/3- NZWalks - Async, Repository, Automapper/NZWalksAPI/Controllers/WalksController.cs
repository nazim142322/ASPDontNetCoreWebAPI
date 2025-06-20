using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly NZWalksDbContext _dbContext;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, NZWalksDbContext dbContext, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _walkRepository = walkRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalksRequestDTO)
        {
            // Validate the request.
            if (addWalksRequestDTO == null)
            {
                return BadRequest("Invalid walk data.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Map the DTO to the Domain Model
                var walkDomainModel = _mapper.Map<Walk>(addWalksRequestDTO);

                //calling the repository to add the walk to the database
                walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);

                //map the Domain Model back to DTO
                var WalkDTO = _mapper.Map<WalkDTO>(walkDomainModel);

                return Ok(WalkDTO);
                //return StatusCode(201, "Record Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {            
                //Get all walks from the database
                var walksDomainModel = await _walkRepository.GetAllWalksAsync();

                //Check if any walks were found
                if (walksDomainModel == null || !walksDomainModel.Any())
                {
                    return NotFound("No walks found.");
                }

                //Map Domain Models to DTOs using AutoMapper
                var walkDTOs = _mapper.Map<IEnumerable<WalkDTO>>(walksDomainModel);
                //Return the list of WalkDTOs
                return Ok(walkDTOs);
            
           
            
        }
    }
}
