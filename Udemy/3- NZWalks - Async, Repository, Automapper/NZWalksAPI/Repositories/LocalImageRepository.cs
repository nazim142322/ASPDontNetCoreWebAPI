namespace NZWalksAPI.Repositories;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using System.Threading.Tasks;


public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _evm;
    private readonly NZWalksDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalImageRepository(IWebHostEnvironment evm, NZWalksDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _evm = evm;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Image> Upload(Image image)
    {
        //combine given file name with extension
        //var fileNameWithExtension = $"{image.FileName}{image.FileExtension}";
        var fileNameWithExtension = Guid.NewGuid().ToString()+"_"+$"{image.FileName}{image.FileExtension}";

        var localFilePath = Path.Combine(_evm.ContentRootPath, "Images", fileNameWithExtension);

        //upload image to local path
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);

        
        //create path - https://localhost:5000/Images/abc.jpg        

        //var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Images/{fileNameWithExtension}";
        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{fileNameWithExtension}";
        image.FilePath = urlFilePath; //set the file path to the image object

        //add the image to the database table
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();
        
        return image;

    }
}

//return new Image
//{
//    Id = image.Id,
//    FileName = image.FileName,
//    Description = image.Description,
//    FileExtension = image.FileExtension,
//    FileSizeInBytes = image.FileSizeInBytes,
//    FilePath = localFilePath
//};

//Query
//MyProject
//├── UploadFiles
//│   └── Images

//var localFilePath = Path.Combine(_evm.ContentRootPath, "UploadFiles", "Images", fileNameWithExtension);

//Explanation:
//_evm.ContentRootPath → aapke project ka root path deta hai.
//"UploadFiles" → root ke andar folder.
//"Images" → uske andar subfolder.
//fileNameWithExtension → jaise "student1.jpg"