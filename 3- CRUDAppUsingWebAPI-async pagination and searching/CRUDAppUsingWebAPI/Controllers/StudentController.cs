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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            List<Student> students = new List<Student>();
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl); //asynchronous call to get data from API//.Result is used for synchronous call, but it is not recommended in production code as it can lead to deadlocks.
            if (response.IsSuccessStatusCode)//200
            {
                string result = await response.Content.ReadAsStringAsync(); //get data in json from response //. Result is used for synchronous call, but it is not recommended in production code as it can lead to deadlocks.
                var data = JsonConvert.DeserializeObject<List<Student>>(result); //convert json data to object
                if (data != null)
                {
                    students = data; //assign data to students list
                }
            }
            // Pagination logic
            int totalStudents = students.Count;
            var pagedStudents = students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalStudents / pageSize);
            return View(pagedStudents);
            //return View(students);
        }

        //Post
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            string data = JsonConvert.SerializeObject(student); //convert student object to json string
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json"); //create content for post request
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content); //send post request to API
            if (response.IsSuccessStatusCode)
            {
                TempData["AddMessage"] = "Student added successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["AddMessage"] = "Student could not be added successfully!";
                //string error = await response.Content.ReadAsStringAsync();
                //TempData["AddMessage"] = "Error: " + error;
                //return View(student);

            }
            return View(student); //if model is valid, return the view with student data
        }

        //update calling api get method
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Student student = new Student();
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + id);//hit the url/apio return response
            if (response.IsSuccessStatusCode)//200
            {
                string result = await response.Content.ReadAsStringAsync();//get data in json from response
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
        public async Task<IActionResult> Edit(int id, Student std)
        {           
            if (!ModelState.IsValid)
            {
                return View(std);
            }
            string data = JsonConvert.SerializeObject(std);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(apiUrl + std.id, content);//hit the api and sending updated data with url
            if (response.IsSuccessStatusCode)
            {
                TempData["UpdateMessage"] = "record updated successfully";
            }
            else
            {
                TempData["UpdateMessage"] = "record could not be updated successfully";                
                //return RedirectToAction("Edit", new { id = std.id }); //returning with wrong id

            }
            return RedirectToAction("Index");            
        }
        //return Json(new { Id =id, record = std});
        //details
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var student = new Student();
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + id); //synchronous call to get data from API
            if (response.IsSuccessStatusCode)//200
            {
                string result = await response.Content.ReadAsStringAsync(); //get data in json from response
                var data = JsonConvert.DeserializeObject<Student>(result); //convert json data to object
                if (data != null)
                {
                    student = data; //assign data to students list
                }
            }
            return View(student);
            //return Json(id);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Student std = new Student();
            HttpResponseMessage response =await _httpClient.GetAsync(apiUrl + id);//synchronous call to get data from API
            if (response.IsSuccessStatusCode)//200
            {
                string result = await response.Content.ReadAsStringAsync(); //get data in json from response
                var data = JsonConvert.DeserializeObject<Student>(result); //convert json data to object
                if (data != null)
                {
                    std = data; //assign data to student object
                }
            }
            return View(std); //returning sid to view for confirmation
        }

        [HttpPost, ActionName("Delete")]       
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl+id); //synchronous call to delete data from API
            if (response.IsSuccessStatusCode) //200
            {
                TempData["DeleteMessage"] = "record deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["DeleteMessage"] = "record could not be deleted successfully";
            }
            return View();
            //return Json(id);
        }        
    }
}
//Newtonsoft.Json;  Json.NET is a popular high - performance JSON framework for .NET