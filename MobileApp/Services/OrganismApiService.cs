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
    }
}
