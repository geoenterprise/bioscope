using System.Net.Http.Json;
using MobileApp.Models;

namespace MobileApp.Services
{
    public class OrganismApiService
    {
        private readonly HttpClient _http;

        public OrganismApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Organism>> GetAllAsync()
        {
            var data = await _http.GetFromJsonAsync<List<Organism>>("api/organisms");
            return data ?? new List<Organism>();
        }

        public Task<Organism?> GetAsync(int id) =>
            _http.GetFromJsonAsync<Organism>($"api/organisms/{id}");

        public async Task<Organism?> PostPhotoAsync(Stream photoStream, string fileName)
        {
            using var content = new MultipartFormDataContent();
            var imageContent = new StreamContent(photoStream);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            content.Add(imageContent, "image", fileName);

            var response = await _http.PostAsync("api/organisms/upload", content); 
            if (response.IsSuccessStatusCode)
            {
                var created = await response.Content.ReadFromJsonAsync<Organism>();
                return created;
            }
            return null;
        }

    }
}
