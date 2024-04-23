using ArtistPortfolio.Models.Models;

namespace ArtistPortfolio.Services.Interfaces
{
    public interface IImageService
    {
        public string ConvertImageToBase64(IFormFile imageFile);
        public byte[] ReadBytesFromStream(Stream input);
        public string UploadedFile(Image image);
    }
}
