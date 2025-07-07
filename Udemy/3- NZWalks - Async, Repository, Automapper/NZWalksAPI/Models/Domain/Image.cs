using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalksAPI.Models.Domain
{
    public class Image
    {

        public Guid Id { get; set; }//by server

        [NotMapped]
        public IFormFile File { get; set; }//ignored by EF Core, used for file upload

        public string FileName { get; set; }//from image upload

        public string? Description { get; set; }//by user

        public string FileExtension { get; set; }//from file upload, e.g., .jpg, .png

        public long FileSizeInBytes { get; set; }//from file upload, size in bytes

        public string FilePath { get; set; } // by server, where the file is stored on the server  


    }
}
