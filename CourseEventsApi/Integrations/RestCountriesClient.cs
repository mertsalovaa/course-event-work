using CourseEventsApi.DTOs;
using CourseEventsApi.Integrations.Interfaces;
using System.Text.Json;

namespace CourseEventsApi.Integrations
{
    public class RestCountriesClient : ICountryClient
    {
        private readonly HttpClient _httpClient;

        public RestCountriesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CountryDTO>> GetAllCountriesAsync()
        {
            var json = await _httpClient.GetStringAsync("https://restcountries.com/v3.1/all");

            var data = JsonSerializer.Deserialize<List<JsonElement>>(json);

            return data.Select(x => new CountryDTO
            {
                Name = x.GetProperty("name").GetProperty("common").GetString(),
                Code = x.GetProperty("cca2").GetString(),
                Region = x.GetProperty("region").GetString()
            }).ToList();
        }

        public async Task<CountryDTO?> GetByCodeAsync(string code)
        {
            var json = await _httpClient.GetStringAsync($"https://restcountries.com/v3.1/alpha/{code}");
            return null; // спростимо поки
        }
    }
}
