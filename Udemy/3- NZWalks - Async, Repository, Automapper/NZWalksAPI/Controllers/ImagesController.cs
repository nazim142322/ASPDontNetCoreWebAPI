using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
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
                FileName = imgRequest.File.FileName,
                FileExtension = Path.GetExtension(imgRequest.File.FileName).ToLowerInvariant(),
                FileSizeInBytes = imgRequest.File.Length,
                Description = imgRequest.Description,
                //file path will be set after saving the file
            };
            return Ok();
        }

        //for image validation
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


        [HttpPost]
        [Route("UploadLocal")]
        public async Task<IActionResult> UploadLocalImage([FromForm] ImageUploadRequestDTO imgRequest)
        {
            if (imgRequest == null || imgRequest.File == null)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                ValidateFileUpload(imgRequest);

                // Save the file to a local folder called "LocalUploads in project root
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "LocalUploads");


                //to check if the folder exists and create it if it doesn't
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imgRequest.File.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imgRequest.File.CopyToAsync(stream);
                }

                return Ok(new { FileName = fileName, FilePath = filePath, Message = "Image uploaded locally." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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