using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using Newtonsoft.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        private string apiBaseUrl = "https://localhost:7050/api/Regions";

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

        public async Task<IActionResult> Get_Regions()
        {
            var regions = new List<RegionDTO>();

            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<RegionDTO>>(content);
                    if (data is not null)
                    {
                        regions = data;
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


    }



}
