
using System.Threading.Tasks;
using api.Constants;
using api.Extensions;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route(RouteConstants.PORTFOLIOS)]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {

            var appUserId = User.GetUserId();
            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUserId);
            return Ok(userPortfolio);
        }

        [HttpPost("stock/{stockId:int}")]
        // POST api/portfolio/stock/1233
        public async Task<IActionResult> AddPortfolio([FromRoute] int stockId)
        {

            var appUserId = User.GetUserId();
            var appUserPortfolio = await _portfolioRepository.AddPortfolioAsync(stockId, appUserId);

            return Ok(appUserPortfolio.Stock);
        }

        [HttpDelete("stock/{stockId:int}")]
        // Delete api/portfolio/stock/1233

        public async Task<IActionResult> DeleteortfolioByStockId([FromRoute] int stockId)
        {
            var appUserId = User.GetUserId();

            var deleted = await _portfolioRepository.DeleteortfolioByStockIdAsync(stockId, appUserId);

            return NoContent();
        }
    }
}
