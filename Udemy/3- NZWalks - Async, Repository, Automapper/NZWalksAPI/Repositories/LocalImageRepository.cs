namespace NZWalksAPI.Repositories;
using NZWalksAPI.Models.Domain;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class LocalImageRepository : ImageRepository
{
    private readonly IWebHostEnvironment _evm;
    public LocalImageRepository(IWebHostEnvironment evm)
    {
        _evm = evm;
    }
    public Task<Image> Upload(Image image)
    {
        var localFilePath = Path.Combine(_evm.WebRootPath, "images", image.FileName);
        throw new NotImplementedException();
    }
}
 