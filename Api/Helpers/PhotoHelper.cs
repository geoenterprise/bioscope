using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PlantAnimalApi.Models;
using System.Text.Json;




namespace MobileApp.Helpers
{
    public static class PhotoHelper
    {
        public static async Task<string> SavePhotoAsync(IFormFile photo)
        {
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}_{photo.FileName}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public static async Task<string> IdentifyPhotoAsync(string filePath)
        {
            try
            {
                string apiKey = "2dYRCiCaBfbLN6xm0I6drq23cZwVEkI88B1UhqmH6L6Idqvq5q";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Api-Key", apiKey);

                using var form = new MultipartFormDataContent();
                var ImageReaded = await File.ReadAllBytesAsync(filePath);
                var imageContent = new ByteArrayContent(ImageReaded);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                form.Add(imageContent, "images[]", Path.GetFileName(filePath));

                var response = await client.PostAsync("https://api.plant.id/v2/identify", form);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return json;

               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
