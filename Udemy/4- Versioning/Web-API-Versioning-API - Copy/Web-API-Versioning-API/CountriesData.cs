using Web_API_Versioning_API.Models.Domain;

namespace Web_API_Versioning_API
{
    public class CountriesData
    {
        public static List<Country> Get()
        {
            var result = new List<Country>
            {
                new Country { Id = 1, Name = "United States" },
                new Country { Id = 2, Name = "Canada" },
                new Country { Id = 3, Name = "Mexico" },
                new Country { Id = 4, Name = "United Kingdom" },
                new Country { Id = 5, Name = "Germany" }

            };
            return result;
        }

        public static List<Country> GetAll()
        {
            //annonymous type array
            var countries = new[]
             {
               new { Id = 1, Name = "United States" },
               new { Id = 2, Name = "Germany" },
               new  { Id = 2, Name = "Canada" },
               new  { Id = 3, Name = "Mexico" },
               new { Id = 4, Name = "United Kingdom" }
            };

            //LINQ query to select the Id and Name properties
            var data = countries.Select(c => new Country { Id = c.Id, Name = c.Name }).ToList();

            return data;
            //return countries.Select(c => new Country { Id = c.Id, Name = c.Name }).ToList();
        }
    }

}