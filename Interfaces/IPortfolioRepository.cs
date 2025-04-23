
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stocks>> GetUserPortfolioAsync(string appUserId);
        Task<Portfolios> AddPortfolioAsync(int stockId, string appUserId);

        Task<Portfolios?> DeleteortfolioByStockIdAsync(int stockId, string appUserId);
    }
}
