namespace Web_API_Versioning_API.Models.DTO
{
    public class CountryDTOV1
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }


    public class CountryDTOV2
    {
        public int Id { get; set; }
        public string? CountryName { get; set; }
    }
}
