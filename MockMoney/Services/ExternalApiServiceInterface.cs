using System.Threading.Tasks;

namespace MockMoney.Services
{
    public interface IExternalAPIService
    {
        Task<string> GetJWTTokenAsync(string login, string password);
        Task<string> GetAllStocksAsync(string token);
        Task<string> BuyStockAsync(string token, int amount, string ticket); 
        Task<string> GetMyStocksAsync(string token); 
        Task<string> SellStockAsync(string token, int amount, string ticket); 
        Task<string> GetSpecificStockAsync(string token, string id); 
        Task<string> GetUserInfoAsync(string token); 
        Task<string> GetPolynomialAsync(string token); 
        Task<string> RegisterAsync(string displayName, string login, string password);
        Task<string> CheckHealthAsync(); 
    }
}
