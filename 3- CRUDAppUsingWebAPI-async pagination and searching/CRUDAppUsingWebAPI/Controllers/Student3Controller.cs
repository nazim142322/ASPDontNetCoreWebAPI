using CRUDAppUsingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace CRUDAppUsingWebAPI.Controllers
{
    public class Student3Controller : Controller
    {
        private string apiUrl = "https://localhost:7004/api/Student/";
        private HttpClient _httpClient = new HttpClient(); //without it we cannot work with api
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1, int pageSize = 5, string sortField = "studnetName", string sortOrder = "asc")
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

            // 🔍 Search
            if (!string.IsNullOrEmpty(searchString))
            {
                students = students
                    .Where(s => !string.IsNullOrEmpty(s.studnetName) &&
                                s.studnetName.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            // 🔃 Sort
            switch (sortField)
            {
                case "age":
                    students = sortOrder == "asc" ? students.OrderBy(s => s.age).ToList()
                                                  : students.OrderByDescending(s => s.age).ToList();
                    break;
                case "standard":
                    students = sortOrder == "asc" ? students.OrderBy(s => s.standard).ToList()
                                                  : students.OrderByDescending(s => s.standard).ToList();
                    break;
                default:
                    students = sortOrder == "asc" ? students.OrderBy(s => s.studnetName).ToList()
                                                  : students.OrderByDescending(s => s.studnetName).ToList();
                    break;
            }

            // 📄 Pagination
            int totalStudents = students.Count;
            var pagedStudents = students.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // 🧠 ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalStudents / pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            return View(pagedStudents);
        }

    }
}
