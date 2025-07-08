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
                //file path will be set later in the repository
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


        [HttpPost]
        [Route("UploadLocal")]        
        public async Task<IActionResult> UploadLocalImage([FromForm] ImageUploadRequestDTO imgRequest) // Image upload method — file frontend form se aayegi
        {
            // Check kar rahe hain ki file null to nahi hai (user ne kuch bheja bhi hai ya nahi)
            if (imgRequest == null || imgRequest.File == null)
            {
                return BadRequest("No file uploaded."); // Agar nahi bheja to error de do
            }

            try
            {
                // File ka type/size valid hai ya nahi check karo (jaise sirf jpg/png allow ho)
                ValidateFileUpload(imgRequest);

                // Upload hone wali image ko kaha save karna hai? => "LocalUploads" folder me
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "LocalUploads");

                // Agar "LocalUploads" naam ka folder exist nahi karta to usse create karo
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder); // pehli baar ke liye folder banta hai
                }

                // Unique naam banate hain file ka (taaki kisi aur ki file overwrite na ho)
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imgRequest.File.FileName)}";

                // Final path jaha image save hogi: e.g., C:/Project/LocalUploads/xyz.jpg
                var filePath = Path.Combine(uploadsFolder, fileName);

                // File ko is path par copy kar rahe hain stream ke through
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imgRequest.File.CopyToAsync(stream); // async tareeke se save ho rahi hai
                }

                // Response return kar rahe hain — file ka naam, path, aur message
                return Ok(new { FileName = fileName, FilePath = filePath, Message = "Image uploaded locally." });
            }
            catch (Exception ex)
            {
                // Agar kuch galti ho gayi to uska message de do
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