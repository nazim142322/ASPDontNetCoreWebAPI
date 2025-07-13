using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;
using System.Net;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestLoggingController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ILogger<TestLoggingController> _Logger;

        public TestLoggingController(IRegionRepository regionRepository, ILogger<TestLoggingController> logger1)
        {
            _regionRepository = regionRepository;
            _Logger = logger1;
        }


        [HttpGet]
        [Route("Serilog")]
        public async Task<IActionResult> SerilogLogging()
        {
            _Logger.LogInformation($"serilog logging is working fine");//it will not work bcoz .MinimumLevel.Warning()

            _Logger.LogWarning($"This is a warning message from Serilog");
            _Logger.LogError($"This is an error message from Serilog");

            //calling repository to get all regions
            var regionsDomain = await _regionRepository.GetAllAsync();

            //it will not work bcoz .MinimumLevel.Warning()
            _Logger.LogInformation($"Finished getting all regions request total records : {regionsDomain.Count()} and records: {System.Text.Json.JsonSerializer.Serialize(regionsDomain)}");
            
            return Ok(regionsDomain);        
        }


        [HttpGet]
        [Route("logException")]
        public async Task<IActionResult> ExceptionHandling()
        {
            try
            {
                throw new Exception("This is a test exception for Serilog logging");

                //calling repository to get all regions
                var regionsDomain = await _regionRepository.GetAllAsync();
                return Ok(regionsDomain);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);//write in console and file bcoz .WriteTo.Console() and .WriteTo.File             
                //_Logger.LogError(ex, "An error occurred while processing the request in ExceptionHandling method");

                //return StatusCode(500, "Internal server error");
                return Problem("Something Went Wrong", null, (int)HttpStatusCode.InternalServerError);
                //throw; //rethrowing the exception to be handled by global exception handler
            }
        }

        [HttpGet]
        [Route("LogsToFile")]
        public async Task<IActionResult> SerilogLogToFile()
        {
            try
            {
                throw new Exception("This is a test exception for Serilog logging");

                //calling repository to get all regions
                var regionsDomain = await _regionRepository.GetAllAsync();
                return Ok(regionsDomain);
            }
            catch (Exception ex)
            {
               // _Logger.LogError(ex, ex.Message);
               _Logger.LogError(ex, "An error occurred while processing the request in ExceptionHandling method");


                //return StatusCode(500, "Internal server error");
                return Problem("Something Went Wrong", null, (int)HttpStatusCode.InternalServerError);
                //throw; //rethrowing the exception to be handled by global exception handler
            }
        }

    }
}
