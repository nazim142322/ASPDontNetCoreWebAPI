using CRUDAppUsingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax; //for json conversion

namespace CRUDAppUsingWebAPI.Controllers
{
    public class StudentController : Controller
    {
        private string apiUrl = "https://localhost:7004/api/Student/";
        private HttpClient _httpClient = new HttpClient(); //without it we cannot work with api

        //Get
        [HttpGet]
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();
            HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result; //synchronous call to get data from API
            if (response.IsSuccessStatusCode)//200
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

        //Post
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
            else
            {
                TempData["AddMessage"] = "Student could not be added successfully!";
            }
            return View(student); //if model is valid, return the view with student data
        }

        //update calling api get method
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student student = new Student();
            HttpResponseMessage response = _httpClient.GetAsync(apiUrl+id).Result;//hit the url/apio return response
            if (response.IsSuccessStatusCode)//200
            {
                string result = response.Content.ReadAsStringAsync().Result;//get data in json from response
                var data = JsonConvert.DeserializeObject<Student>(result);//convert(result) json data to object(Student)
                if (data != null)
                {
                    student = data; //assign data to student object
                }
            }
            return View(student);
        }
        //updating records put request
        [HttpPost]
        public IActionResult Edit(int id, Student std)
        {
            //if (id!= std.id)
            //{
            // return Json(new { Id = id, record = std });
            //return View("Edit", std);
            // }
            if (!ModelState.IsValid)
            {
                return View(std);
            }
            string data = JsonConvert.SerializeObject(std);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(apiUrl + std.id, content).Result;//hit the api and sending updated data with url
            if(response.IsSuccessStatusCode)
            {
                TempData["UpdateMessage"] = "record updated successfully";
            }
            else
            {
                TempData["AddMessage"] = "record could not be updated successfully";
                
            }
            return RedirectToAction("Index");
            //return Json(new { Id =id, record = std});
        }
        //details
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var student = new Student();
            HttpResponseMessage response = _httpClient.GetAsync(apiUrl+id).Result; //synchronous call to get data from API
            if (response.IsSuccessStatusCode)//200
            {
                string result = response.Content.ReadAsStringAsync().Result; //get data in json from response
                var data = JsonConvert.DeserializeObject<Student>(result); //convert json data to object
                if (data != null)
                {
                    student = data; //assign data to students list
                }
            }
            return View(student);
            //return Json(id);
        }

        //to Delete
        [HttpGet]
        public IActionResult Delete(int sid)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync(apiUrl + sid).Result; //synchronous call to get data from API
            if (response.IsSuccessStatusCode)//200
            {
                TempData["DeleteMessage"] = "record deleted successfully";
                return RedirectToAction("Index");
            }
            return View();
                //return Json(sid);
        }

    }
}
//Newtonsoft.Json;  Json.NET is a popular high - performance JSON framework for .NET