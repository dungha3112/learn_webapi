

using api.Data;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;

        public PortfolioRepository(ApplicationDBContext context, UserManager<AppUser> userManager, IMapper mapper, IStockRepository stockRepository)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;

            _stockRepository = stockRepository;
        }

        public async Task<Portfolios> AddPortfolioAsync(int stockId, string userName)
        {
            if (userName == null) throw new UnauthorizedAccessException("Unauthorized");

            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null) throw new UnauthorizedAccessException("Unauthorized");


            var stock = await _stockRepository.StockExistsAsync(stockId);
            if (!stock) throw new DllNotFoundException("Stock symbol not found");

            var checkSymbolExist = await SymbolAlreadyExistsPortfolio(userName, stockId);
            if (checkSymbolExist) throw new InvalidOperationException("Can not add same stock to portfolio");

            var portfolioModel = new Portfolios
            {
                AppUserId = appUser.Id,
                StockId = stockId,
            };

            await _context.Portfolios.AddAsync(portfolioModel);
            var savedCount = await _context.SaveChangesAsync();
            if (savedCount == 0)
                throw new Exception("Can not create new portfolio");

            return portfolioModel;
        }

        public async Task<bool> SymbolAlreadyExistsPortfolio(string userName, int stockId)
        {
            var userPortfolio = await GetUserPortfolioAsync(userName);
            // Console.WriteLine("-------------------- SymbolAlreadyExistsPortfolio ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(userPortfolio, Formatting.Indented));
            // Console.WriteLine("stockId: " + stockId);
            var check = userPortfolio.Any(e => e.Id == stockId);

            return check;
        }


        public async Task<List<Stocks>> GetUserPortfolioAsync(string userName)
        {
            if (userName == null) throw new UnauthorizedAccessException("Unauthorized");

            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null) throw new UnauthorizedAccessException("Unauthorized");

            // Console.WriteLine("-------------------- app user ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(appUser, Formatting.Indented));


            var userPortfolio = await _context.Portfolios.Where(u => u.AppUserId == appUser.Id)
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
            // Console.WriteLine("-------------------- userPortfolio ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(userPortfolio, Formatting.Indented));

            // var userPortfolioDtos = _mapper.Map<List<StockDto>>(userPortfolio);
            // Console.WriteLine("-------------------- user Portfolio dto ---------------------");
            // Console.WriteLine(JsonConvert.SerializeObject(userPortfolioDtos, Formatting.Indented));
            return userPortfolio;
        }
    }
}
