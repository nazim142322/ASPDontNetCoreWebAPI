using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDTO imgRequest)
        {
            ValidateFileUpload(imgRequest);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //convert dto to domain model since repository layer works with domain models
            var imageDomainModel = new Image
            {
                File = imgRequest.File,//Iform file                
                FileExtension = Path.GetExtension(imgRequest.File.FileName).ToLowerInvariant(),
                FileSizeInBytes = imgRequest.File.Length,
                FileName = imgRequest.FileName,
                Description = imgRequest.Description,
                //FilePath will be set later in the repository
            };
            // Call the repository to save the image
            await _imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        //for image validation extension and size
        private void ValidateFileUpload(ImageUploadRequestDTO imgRequest)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(imgRequest.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                //throw new Exception("Invalid file type. Only .jpg, .jpeg, and .png files are allowed.");
                ModelState.AddModelError("File", "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.");
            }
            if (imgRequest.File.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                //throw new Exception("File size exceeds the maximum limit of 5 MB.");
                ModelState.AddModelError("File", "File size exceeds the maximum limit of 5 MB.");
            }
        }
    }
}

//To store uploads inside wwwroot, use:
//var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LocalUploads");

// Save the file to a local folder called "LocalUploads in project root
//var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "LocalUploads");

//Summary:
//Directory.GetCurrentDirectory(), "LocalUploads" → project root
//Directory.GetCurrentDirectory(), "wwwroot", "LocalUploads" → inside wwwroot (recommended for serving files)      