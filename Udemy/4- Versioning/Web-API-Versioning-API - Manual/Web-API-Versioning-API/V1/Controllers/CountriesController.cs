using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning_API.Models.DTO;

namespace Web_API_Versioning_API.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCountries()
        {            
            var countriesDomainModel = CountriesData.GetAll();

            var response = new List<CountryDTOV1>();

            foreach( var countryDomain in countriesDomainModel)
            {
                var country = new CountryDTOV1
                {
                    Id = countryDomain.Id,
                    Name = countryDomain.Name
                };
                response.Add(country);

                //response.Add(new CountryDTO
                //{
                //    Id = countryDomain.Id,
                //    Name = countryDomain.Name
                //});
            }
            return Ok(response);
        }
    }
}
