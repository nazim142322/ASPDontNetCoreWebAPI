using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication12.Models;

namespace WebApplication12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DbwebapiContext _dbContext;
        public StudentController(DbwebapiContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet("GetAllStudents")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var data = await _dbContext.Students.ToListAsync();
            return Ok(data);//convert list to JSON
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            var student = await _dbContext.Students.FindAsync(Id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)//"[FormBody] JSON ko Student object me convert karo."
        {
            if (student == null)
            {
                return BadRequest("Student data is null");
            }
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return Ok(student); // For simplicity, returning Ok instead of CreatedAtAction
        }

        [HttpPut("UpdateRecord/{Id}")]
        public async Task<IActionResult> UpdateStudentById(int Id, [FromBody] Student std)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == Id);
            if (student == null)
            {
                return NotFound();
            }
            student.StudnetName = std.StudnetName;
            student.StudentGender = std.StudentGender;
            student.Age = std.Age;
            student.Standard = std.Standard;
            student.FatherName = std.FatherName;
            await _dbContext.SaveChangesAsync();
            return Ok(student);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateStudent(int Id, [FromBody] Student std)
        {
            if(Id!=std.Id)
            {
                return BadRequest("Student ID mismatch");
            }
            _dbContext.Entry(std).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(std); // For simplicity, returning Ok instead of CreatedAtAction

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            var student = await _dbContext.Students.FindAsync(Id);
            if(student == null)
            {
                return NotFound("Record not found in db");
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Student deleted successfully" });

        }
    }

    //Entity Framework ko ye batata hai ki object std ek pehle se existing record hai, aur ab change ho gaya hai, isliye isse update karo using its Id.
}   // return Ok(...) pass object and string
