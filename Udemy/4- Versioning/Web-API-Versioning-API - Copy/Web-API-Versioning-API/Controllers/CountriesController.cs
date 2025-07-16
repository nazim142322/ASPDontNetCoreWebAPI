using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning_API.Models.DTO;

namespace Web_API_Versioning_API.V1.Controllers
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult GetCountriesV1()
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


        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult GetCountriesV2()
        {
            var countriesDomainModel = CountriesData.GetAll();

            var response = new List<CountryDTOV2>();

            foreach (var countryDomain in countriesDomainModel)
            {
                var country = new CountryDTOV2
                {
                    Id = countryDomain.Id,
                    CountryName = countryDomain.Name
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
