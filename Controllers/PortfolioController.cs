
using api.Constants;
using api.Dtos.Portfolio;
using api.Extensions;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route(RouteConstants.PORTFOLIOS)]
    [ApiController]

    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;

        public PortfolioController(IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {

            var username = User.GetUsername();
            Console.WriteLine("username: " + username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(username);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromBody] AddPortfolioRequestDto addPortfolioRequest)
        {

            var username = User.GetUsername();
            var stock = await _portfolioRepository.AddPortfolioAsync(addPortfolioRequest.StockId, username);

            return Ok(stock);
        }
    }
}
