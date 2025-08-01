﻿using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<RegionsController> _logger;
        public RegionsController(IHttpClientFactory httpClientFactory, ILogger<RegionsController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        private string apiBaseUrl = "https://localhost:7050/api/Regions/";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var _httpClient = httpClientFactory.CreateClient();

            List<RegionDTO> regions = new List<RegionDTO>();
            HttpResponseMessage response = await _httpClient.GetAsync(apiBaseUrl);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();// geeting json string from the api resonse
                var data = JsonConvert.DeserializeObject<List<RegionDTO>>(content);//converting json string to list of RegionDTO
                if (data is not null)
                {
                    regions = data;
                }
            }
            return View(regions); //in case no data then blank list will be returned
        }


        //get method to get regions with try catch block and using ReadFromJsonAsync
        //[HttpGet("GetRegions")] //hide controller and method name
        //[HttpGet]
        [HttpGet, ActionName("Get_Regions")]
        public async Task<IActionResult> GetAll()
        {
            var regions = new List<RegionDTO>();

            try
            {
                var _client = httpClientFactory.CreateClient();
                HttpResponseMessage response = await _client.GetAsync(apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var regionsData = await response.Content.ReadFromJsonAsync<List<RegionDTO>>();

                    if (regionsData is not null)
                    {
                        regions = regionsData;
                    }
                }
                else
                {
                    TempData["Error"] = "Failed to load regions. Status: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching regions: " + ex.Message;
            }
            return View(regions);
        }

        //for creating a record POST Request
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRegionViewModel region)
        {
            if (!ModelState.IsValid)
            {
                return View(region);
            }

            try
            {
                var _httpClient = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiBaseUrl),
                    Content = new StringContent(JsonConvert.SerializeObject(region), System.Text.Encoding.UTF8,
                        "application/json"
                    )
                };

                HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode)
                {
                    //getting content from the API response
                    var data = await response.Content.ReadFromJsonAsync<RegionDTO>();
                    TempData["Success"] = "Region created successfully. Status: " + response.StatusCode;
                    //return RedirectToAction("Index");
                    return View();
                }
                else
                {

                    // 👇 getting error content from API response
                    string errorContent = await response.Content.ReadAsStringAsync();

                    // Log the error details
                    _logger.LogError($"Failed to Create Region. Status :{response.StatusCode} and Reason:{errorContent}");

                    TempData["Error"] = "Failed to create region. Status: " + response.StatusCode +
                                        ". Reason: " + errorContent;
                    return View(region);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError($"An error occurred while creating the region: {ex.Message}");

                TempData["Error"] = "An error occurred while creating the region: " + ex.Message;
                return View(region);
            }
        }


        //for updating record PUT Request
        //1 approach GetAsync and ReadFromJsonAsync
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1. Basic validation
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Invalid region ID.";
                return RedirectToAction("Index");
            }

            // 2. Prepare model and HttpClient
            //RegionDTO region = new RegionDTO();
            var _httpClient = httpClientFactory.CreateClient();

            try
            {
                // 3. Call API to get region by ID
                var response = await _httpClient.GetAsync(apiBaseUrl + id);

                if (response.IsSuccessStatusCode)
                {
                    // ✅ 4. Deserialize response
                    var regionData = await response.Content.ReadFromJsonAsync<RegionDTO>();
                    if (regionData != null)
                    {
                        //region = regionData;
                        //return View(RegionDTO)
                        return View(regionData);
                    }
                    return View(null);

                }
                else
                {
                    // 👇 Handle API error response
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to load region. Status: {response.StatusCode}, Reason: {errorContent}");

                    TempData["Error"] = "Failed to load region. Status: " + response.StatusCode;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //add logger
                _logger.LogError($"An error occurred while fetching the region: {ex.Message}");

                //5. Handle network or unexpected errors
                TempData["Error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        //2nd approach GetFromJsonAsync
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Invalid region ID.";
                return RedirectToAction("Index");
            }
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7050/api/Regions/{id}");
            if (response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AddRegionViewModel region)
        {
            if (!ModelState.IsValid)
            {
                return View(region);
            }
            try
            {
                var _httpClient = httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(apiBaseUrl + id),
                    Content = new StringContent(JsonConvert.SerializeObject(region), System.Text.Encoding.UTF8,
                        "application/json"
                    )
                };
                HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    //getting content from the API response/ update record
                    var responseData = await response.Content.ReadFromJsonAsync<RegionDTO>();
                    TempData["Success"] = "Region updated successfully. Status: " + response.StatusCode;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    // Log the error details
                    _logger.LogError($"Failed to update region. Status: {response.StatusCode}, Reason: {errorContent}");
                    TempData["Error"] = "Failed to update region. Status: " + response.StatusCode +
                                        ". Reason: " + errorContent;
                    return View(region);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating the region: {ex.Message}");
                TempData["Error"] = "An error occurred while updating the region: " + ex.Message;
                return View(region);
            }
        }


        //for deleting record DELETE Request
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Error"] = "Invalid region ID.";
                return RedirectToAction("Index");
            }
            var _httpClient = httpClientFactory.CreateClient();
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(apiBaseUrl + id);
                if (response.IsSuccessStatusCode)
                {
                    //var responseData = await response.Content.ReadAsStringAsync();
                    //TempData["Success"] = "Region deleted successfully. Status: " + response.StatusCode + responseData;
                    var responseData = await response.Content.ReadFromJsonAsync<RegionDTO>();
                    TempData["Success"] = "Region deleted successfully. Status: " + response.StatusCode + " Id: "+ responseData.Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to delete region. Status: {response.StatusCode}, Reason: {errorContent}");
                    TempData["Error"] = "Failed to delete region. Status: " + response.StatusCode +
                                        ". Reason: " + errorContent;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the region: {ex.Message}");
                TempData["Error"] = "An error occurred while deleting the region: " + ex.Message;
                return RedirectToAction("Index");
            }



        }
    }
}
