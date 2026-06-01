using CourseEventsApi.DTOs;

namespace CourseEventsApi.Integrations.Interfaces
{
    public interface ICountryClient
    {
        Task<List<CountryDTO>> GetAllCountriesAsync();
        Task<CountryDTO?> GetByCodeAsync(string code);
    }
}
