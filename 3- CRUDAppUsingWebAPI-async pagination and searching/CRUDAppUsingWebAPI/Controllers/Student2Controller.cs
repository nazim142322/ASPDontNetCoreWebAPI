using CRUDAppUsingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRUDAppUsingWebAPI.Controllers
{
    public class Student2Controller : Controller
    {
        private string apiUrl = "https://localhost:7004/api/Student/";
        private HttpClient _httpClient = new HttpClient(); //without it we cannot work with api
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1, int pageSize = 5)
        {
            List<Student> students = new List<Student>();

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Student>>(result);
                if (data != null)
                {
                    students = data;
                }
            }

            // 🔍 Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                students = students
                    .Where(s => !string.IsNullOrEmpty(s.studnetName) &&
                                s.studnetName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            // 📄 Pagination
            int totalStudents = students.Count;
            var pagedStudents = students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalStudents / pageSize);
            ViewBag.SearchString = searchString;

            return View(pagedStudents);
        }

    }
}
