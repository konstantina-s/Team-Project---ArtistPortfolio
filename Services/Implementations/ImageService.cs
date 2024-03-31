using ArtistPortfolio.Models.Models;
using ArtistPortfolio.Services.Interfaces;

namespace ArtistPortfolio.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string ConvertImageToBase64(IFormFile imageFile)
        {
            var base64String = Convert.ToBase64String(ReadBytesFromStream(imageFile.OpenReadStream()));

            return base64String;
        }

        public byte[] ReadBytesFromStream(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public string UploadedFile(Image image)
        {
            string uniqueFileName = null!;

            if (image.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = image.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
