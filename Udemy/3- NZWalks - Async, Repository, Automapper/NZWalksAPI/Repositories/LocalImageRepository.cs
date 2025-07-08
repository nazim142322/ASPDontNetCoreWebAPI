namespace NZWalksAPI.Repositories;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using System.Threading.Tasks;


public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _evm;
    private readonly NZWalksDbContext _dbContext;

    public LocalImageRepository(IWebHostEnvironment evm, NZWalksDbContext dbContext)
    {
        _evm = evm;
        _dbContext = dbContext;
    }

    public async Task<Image> Upload(Image image)
    {
        //combine given file name with extension
        var fileNameWithExtension = $"{image.FileName}{image.FileExtension}";

        var localFilePath = Path.Combine(_evm.ContentRootPath, "Images", fileNameWithExtension);

        //upload image to local path
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);

        //https://localhost:5000/Images/abc.jpg
        image.FilePath = $"{_evm.WebRootPath}/Images/{fileNameWithExtension}";
        //add the image to the database table
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();
        
        return image;

    }
}
//return the image
//return new Image
//{
//    Id = image.Id,
//    FileName = image.FileName,
//    Description = image.Description,
//    FileExtension = image.FileExtension,
//    FileSizeInBytes = image.FileSizeInBytes,
//    FilePath = localFilePath
//};