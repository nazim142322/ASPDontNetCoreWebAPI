using Microsoft.AspNetCore.Mvc;

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
            var _httpClient =  httpClientFactory.CreateClient();

            //List<Regions> regions = new List<Regions>();
            HttpResponseMessage response = await _httpClient.GetAsync(apiBaseUrl);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();// geeting json string from the api resonse
                var regions = 
               

            }
            return View();
            //return View("Error", new { message = "Unable to retrieve regions." });

        }
    }
}
