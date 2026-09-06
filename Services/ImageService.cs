namespace Products.Services
{
    public class ImageService
    {
        public async Task<string?> SaveImageAsync(IFormFile file, string dirPath)
        {
            try
            {
                var types = file.ContentType.Split("/");

                if (types.Length != 2 || types[0] != "image")
                {
                    return null;
                }

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string guid = Guid.NewGuid().ToString();
                string ext = Path.GetExtension(file.FileName);
                string imageName = guid + ext;
                string imagePath = Path.Combine(dirPath, imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(fileStream);
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}