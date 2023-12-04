using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MockMoney.Services
{
   //Лучше не переиспользовать клиент (он течет)
   
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly HttpClient _httpClient;

        public ExternalAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("http://casaos.kkpin.online/");
        }

        public async Task<string> GetJWTTokenAsync(string login, string password)
        {
            var requestBody = new { login, password };
            Console.WriteLine($"Request body: {requestBody}"); // Вывод токена в консоль
            var response = await _httpClient.PostAsJsonAsync("/api/v1/internal/login", requestBody);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllStocksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/v1/internal/marketplace");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> BuyStockAsync(string token, int amount, string ticket)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var requestBody = new { amount, ticket };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/internal/marketplace/buy", requestBody);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMyStocksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/v1/internal/marketplace/my-stocks");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SellStockAsync(string token, int amount, string ticket)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var requestBody = new { amount, ticket };
            var response = await _httpClient.DeleteAsync("/api/v1/internal/marketplace/sell");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetSpecificStockAsync(string token, string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/v1/internal/marketplace/stock?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserInfoAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/v1/internal/marketplace/user-info");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPolynomialAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/v1/internal/polynomial");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RegisterAsync(string displayName, string login, string password)
        {
            var requestBody = new { display_name = displayName, login, password };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/internal/register", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return "Регистрация не удалась.";
            }

            return "Пользователь успешно зарегистрирован.";
        }

        public async Task<string> CheckHealthAsync()
        {
            var response = await _httpClient.GetAsync("/health");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
