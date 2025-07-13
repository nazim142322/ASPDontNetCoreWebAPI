using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobaleExceptionHandeling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // Dependency injection for ILogger
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        string[] students =  new string[] { "John", "Jane", "Doe" };


        //exception handling but no exception capturing
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("Error occured");
                Console.WriteLine(DateTime.Today);
                Console.WriteLine("Hello world");
                return Ok(students);
            }
            catch (Exception ex)
            {
                // Error ko user ke liye handle karein
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching students.");
            }
        }
        //user getting error message but developer has no clue about the error....solution to this logging



        //exception handling and capturing exception
        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> GetAllSutdents()
        {
            _logger.LogInformation("GetAllStudents method called at {time}", DateTime.UtcNow);

            try
            {
                throw new Exception("Error occured");                
                return Ok(students);
            }
            catch (Exception ex)
            {
                // 👇 Ye line error ko console ya file me log karegi
                _logger.LogError(ex, "An error occurred in GetAllStudents action while fetching students at {time}", DateTime.UtcNow);

                // Error ko user ke liye handle karein
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching students.");
            }
        }

    }
}
//return StatusCode(500, "error occured");//Hardcoded value
//return StatusCode(StatusCodes.Status500InternalServerError, "error occured");//Named constant use

//Mehtod-1
//1- console is working fine without any error
//2- if an exception is occured then it is not capturing in console for developer..

//method-2
//Solution is logging
//By default, ASP.NET Core project me basic logging system enabled hota hai, lekin: