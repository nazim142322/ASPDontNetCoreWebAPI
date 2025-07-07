using System.Net;
using NZWalksAPI.Models.Domain;
namespace NZWalksAPI.Repositories
{
    public interface ImageRepository
    {
       Task<Image> Upload(Image image);
    }
}
