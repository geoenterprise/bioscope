using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PlantAnimalApi.Models;
using System.Text.Json;
using dotenv.net;






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
            
                DotEnv.Load();
                string apiKey = Environment.GetEnvironmentVariable("PLANT_ID_API_KEY");
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Api-Key", apiKey);

                // using var form = new MultipartFormDataContent();

                var ImageInBytes = await File.ReadAllBytesAsync(filePath);
                string base64Image = Convert.ToBase64String(ImageInBytes);

                var requestBody = new
                {
                    images = new[] { base64Image },
                    modifiers = new[] { "crops_fast", "similar_images" },
                    plant_details = new[] { "common_names", "url", "wiki_description", "taxonomy", "edible_parts" }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");


                // var imageContent = new ByteArrayContent(ImageReaded);
                // imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                // form.Add(imageContent, "images[]", Path.GetFileName(filePath));

                var response = await client.PostAsync("https://api.plant.id/v2/identify", jsonContent);
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
