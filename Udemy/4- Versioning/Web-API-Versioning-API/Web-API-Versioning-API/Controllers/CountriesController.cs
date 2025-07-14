using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning_API.Models.DTO;

namespace Web_API_Versioning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCountries()
        {            
            var countriesDomainModel = CountriesData.GetAll();

            var response = new List<CountryDTO>();

            foreach( var counrtyDomain in countriesDomainModel)
            {
                response.Add(new CountryDTO
                {
                    Id = counrtyDomain.Id,
                    Name = counrtyDomain.Name
                });
            }
            return Ok(response);
        }
    }
}
