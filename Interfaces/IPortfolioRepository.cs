
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stocks>> GetUserPortfolioAsync(string userName);
        Task<Portfolios> AddPortfolioAsync(int stockId, string userName);
    }
}
