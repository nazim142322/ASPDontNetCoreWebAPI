using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Globale_Exception_Handeling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        
        string[] students = new string[]
        {
            "John Doe",
            "Jane Smith",
            "Alice Johnson"
        };


        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(students);
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
       
            throw new Exception("An error occurred while fetching students in GetAllStudents Action.");
            return Ok(students);
        }
    }
}
//1- create a customer middleware class for global exception handling
//2- register the middleware in Program.cs
//3- create a controller with some endpoints
//4- throw an exception in one of the endpoints to test the middleware
//5- run the application and test the endpoints
//6- check the response for the endpoint that throws an exception
