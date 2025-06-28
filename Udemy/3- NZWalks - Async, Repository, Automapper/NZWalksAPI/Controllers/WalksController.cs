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
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalksRequestDTO)
        {
            // Validate the request.
            if (addWalksRequestDTO == null)
            {
                return BadRequest("Invalid walk data.");
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
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

        //Get Walks
        //Get: /api/Walks
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


        //Get Walks filer
        //Get:/api/walks?filterOn=Name&filterQuery=Track
        [HttpGet("Filttering")]
        public async Task<IActionResult> GetAllbyFilter([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            //Get all walks from the database
            var walksDomainModel = await _walkRepository.GetWalksByFilterAsync(filterOn, filterQuery);

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

        //filter and sorting
        //Get:/api/walks?filterOn=Name&filterQuery=Track&SortBy=Name&isActive=true
        [HttpGet("FiltteringSorting")]
        public async Task<IActionResult> GetAllbyFilterSorting([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            //Get all walks from the database
            var walksDomainModel = await _walkRepository.GetWalksByFilterSortingAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

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

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Check if the id is valid
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid walk ID.");
            }

            //Get the walk by id from the database
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound($"Walk with ID {id} not found.");
            }

            //map the Domain Model to DTO
            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDomainModel);
        }

        [HttpPut("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestdto)
        {
            if (updateWalkRequestdto == null)
            {
                return BadRequest();
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // map the DTO to the Domain Model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestdto);

            //call the repository to update the walk
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            //check if the walk exists            
            if (walkDomainModel == null)
            {
                return NotFound($"Walk with ID {id} not found.");
            }
            // map the updated Domain Model to DTO
            var updatedWalkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(updatedWalkDTO);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Check if the id is valid
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid walk ID.");
            }
            //call the repository to delete the walk
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

            //check if the walk exists
            if (deletedWalkDomainModel == null)
            {
                return NotFound($"Walk with ID {id} not found.");
            }
            //return the deleted walk details
            var deletedWalkDTO = _mapper.Map<WalkDTO>(deletedWalkDomainModel);
            return Ok(deletedWalkDTO);
        }
    }
}
