using CRUDAppUsingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Newtonsoft.Json; //for json conversion

namespace CRUDAppUsingWebAPI.Controllers
{
    public class StudentController : Controller
    {
        private string apiUrl = "https://localhost:7004/api/Student/";
        private HttpClient _httpClient = new HttpClient(); //without it we cannot work with api

        [HttpGet]
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();
            HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result; //synchronous call to get data from API
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result; //get data in json from response
                var data = JsonConvert.DeserializeObject<List<Student>>(result); //convert json data to object
                if (data != null)
                {
                    students = data; //assign data to students list
                }   
            }
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if(!ModelState.IsValid)
            {
                return View(student);
            }
            string data = JsonConvert.SerializeObject(student); //convert student object to json string
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json"); //create content for post request
            HttpResponseMessage response = _httpClient.PostAsync(apiUrl, content).Result; //send post request to API
            if (response.IsSuccessStatusCode)
            {
                TempData["AddMessage"] = "Student added successfully!";
                return RedirectToAction("Index"); 
            }
            return View(student); //if model is valid, return the view with student data
        }
    }
}
//Newtonsoft.Json;  Json.NET is a popular high - performance JSON framework for .NET