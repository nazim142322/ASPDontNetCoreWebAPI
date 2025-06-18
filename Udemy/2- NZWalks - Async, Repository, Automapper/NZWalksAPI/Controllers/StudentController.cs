using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class StudentController : ControllerBase
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
    }
}
