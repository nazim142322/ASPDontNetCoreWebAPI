using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using Newtonsoft.Json;

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
        private string apiBaseUrl = "https://localhost:7050/api/Regions";

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
        //[HttpGet("GetRegions")]
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
    
    }
}
//PostAsJsonAsync, PostAsync and SendAsync
//public async Task<IActionResult> Create(AddRegionViewModel region)
//{
//    if (!ModelState.IsValid)
//    {
//        return View(region);
//    }
//    var _httpClient = httpClientFactory.CreateClient();
//    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiBaseUrl, region);
//    or
//    var response = await _httpClient.PostAsJsonAsync(apiBaseUrl, region);
//    if (response.IsSuccessStatusCode)
//    {
//        TempData["Success"] = "Region created successfully.";
//        return RedirectToAction("Index");
//    }
//    else
//    {
//        TempData["Error"] = "Failed to create region. Status: " + response.StatusCode;
//        return View(region);
//    }
//}