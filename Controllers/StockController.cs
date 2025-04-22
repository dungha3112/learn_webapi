
using api.Constants;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route(RouteConstants.Stocks)]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        //GET /api/stocks?page=2&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StockQueryObject query)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _stockRepository.GetAllAsync(query);
            return Ok(response);

        }


        [HttpGet("{id:int}")]
        //GET /api/stocks/id
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) return NotFound("Stock Id not found");
            return Ok(stock);
        }

        [HttpPost]
        //POST /api/stocks/
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto createStockDto)
        {

            var stockDto = await _stockRepository.CreateAsync(createStockDto);
            return CreatedAtAction(nameof(GetById), new { id = stockDto.Id }, stockDto);
        }

        [HttpPut("{id:int}")]
        //PUT /api/stocks/id
        public async Task<IActionResult> UpdatetById([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
        {

            var stock = await _stockRepository.UpdateByIdAsync(id, updateStockDto);
            if (stock == null) return NotFound("Stock Id not found");

            return Ok(stock);
        }

        [HttpDelete("{id:int}")]
        //DELETE /api/stocks/id
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {

            var deletedStock = await _stockRepository.DeleteByIdAsync(id);
            if (deletedStock == null) return NotFound("Stock Id not found");

            return Ok(deletedStock);

        }
    }


}
