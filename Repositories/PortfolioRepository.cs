

using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly IStockRepository _stockRepository;

        public PortfolioRepository(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _context = context;


            _stockRepository = stockRepository;
        }

        public async Task<Portfolios> AddPortfolioAsync(int stockId, string appUserId)
        {
            if (appUserId == null) throw new UnauthorizedAccessException("Unauthorized");


            // Console.WriteLine("----------------- AddPortfolioAsync appuser ---------------");
            // Console.WriteLine(JsonConvert.SerializeObject(appUser, Formatting.Indented));


            var stock = await _stockRepository.StockExistsAsync(stockId);
            if (!stock) throw new KeyNotFoundException("Stock Id not found");

            var checkSymbolExist = await SymbolAlreadyExistsPortfolio(appUserId, stockId);
            if (checkSymbolExist) throw new InvalidOperationException("Can not add same stock to portfolio");

            var portfolioModel = new Portfolios
            {
                AppUserId = appUserId,
                StockId = stockId,
            };

            await _context.Portfolios.AddAsync(portfolioModel);
            var savedCount = await _context.SaveChangesAsync();
            if (savedCount == 0)
                throw new Exception("Can not create new portfolio");

            return portfolioModel;
        }

        public async Task<bool> SymbolAlreadyExistsPortfolio(string appUserId, int stockId)
        {
            var userPortfolio = await GetUserPortfolioAsync(appUserId);
            // Console.WriteLine("-------------------- SymbolAlreadyExistsPortfolio ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(userPortfolio, Formatting.Indented));
            // Console.WriteLine("stockId: " + stockId);
            var check = userPortfolio.Any(e => e.Id == stockId);

            return check;
        }


        public async Task<List<Stocks>> GetUserPortfolioAsync(string appUserId)
        {
            if (appUserId == null) throw new UnauthorizedAccessException("Unauthorized");


            // Console.WriteLine("-------------------- app user ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(appUser, Formatting.Indented));


            var userPortfolio = await _context.Portfolios.Where(u => u.AppUserId == appUserId)
                // .Select(p => p.Stock) // get entity Stock
                .Select(stock => new Stocks
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap
                })
                .ToListAsync();
            return userPortfolio;
        }

        public async Task<Portfolios?> DeleteortfolioByStockIdAsync(int stockId, string appUserId)
        {
            var stock = await _stockRepository.StockExistsAsync(stockId);
            if (!stock) throw new KeyNotFoundException("Stock Id not found");

            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.StockId == stockId);
            if (portfolioModel == null)
                throw new KeyNotFoundException("Portfolio entry not found");

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();

            return portfolioModel;

        }
    }
}
