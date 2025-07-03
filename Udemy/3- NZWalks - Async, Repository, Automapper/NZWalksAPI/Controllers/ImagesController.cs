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

            //convert dto to domain model
            var imageDomainModel = new Image
            {
                File = imgRequest.File,
                FileName = imgRequest.FileName,
                FileExtension = Path.GetExtension(imgRequest.File.FileName).ToLowerInvariant(),
                FileSizeInBytes = imgRequest.File.Length,
                Description = imgRequest.Description,
            };





            return Ok();
        }
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

      