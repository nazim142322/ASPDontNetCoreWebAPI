using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestGlobalMiddlewareController : ControllerBase
    {
        [HttpGet]
        [Route("testException")]
        public async Task <IActionResult> TestException()
        {
            //direct code, no try-catch needed            
            string[] students = new string[] { "sanjay", "anil", "shaily", "Nazim" };
            return Ok(students[5]); // this will throw an exception because index 5 is out of range
        }


        [HttpGet]
        [Route("testNotFound")]
        public IActionResult TestNotFound()
        {
            //direct code, no try-catch needed
            throw new Exception("Nazim Exception message2");
            string[] students = new string[] { "sanjay", "anil", "shaily", "Nazim" };
            return Ok(students); 
        }
    }
}
//steps to create a global exception handler middleware in ASP.NET Core Web API and test it
//1- first create a middleware class named GlobalExcepHandlerMiddleware.cs in Middlewares folder
//2- then register it in Program.cs file
//3- then create a controller named TestGlobalMiddlewareController.cs in Controllers folder
//4- then create a test method in TestGlobalMiddlewareController.cs to test the global exception handler middleware
