
using api.Constants;
using api.Dtos.Comment;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route(RouteConstants.COMMENTS)]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }


        [HttpGet("{id:int}")]
        //GET /api/comments/{id}
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            var comment = await _commentRepository.GetCommentByIdAsync(id);

            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpGet("stocks/{stockId:int}")]
        //Get /api/comments/stocks/{stockId}?page=?&pageSize=???
        public async Task<IActionResult> GetCommentsByStockId([FromRoute] int stockId)
        {

            var comments = await _commentRepository.GetCommentsByStockIdAsync(stockId);

            return Ok(comments);
        }


        [HttpPost("stocks/{stockId:int}")]
        //POST /api/comments/stocks/{stockId}
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentRequestDto)
        {

            var stockExists = await _stockRepository.StockExistsAsync(stockId);
            if (!stockExists) return BadRequest("Stock Id does not exists.");

            var commentDto = await _commentRepository.CreateCommentByStockIdAsync(stockId, commentRequestDto);


            return CreatedAtAction(nameof(GetById), new { id = commentDto.Id }, commentDto);
        }

        [HttpPut("stocks/{stockId:int}")]
        //PUT /api/comments/stocks/{stockId}
        public async Task<IActionResult> UpdateComment([FromRoute] int stockId, [FromBody] UpdateCommentRequestDto commentRequestDto)
        {

            var commentDto = await _commentRepository.UpdateCommentByStockIdAsync(stockId, commentRequestDto);
            if (commentDto == null) return NotFound("Stock Id not found");

            return Ok(commentDto);
        }


        [HttpDelete("{id:int}")]
        //PUT /api/comments/id
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {

            var comment = await _commentRepository.DeleteCommentByIdAsync(id);
            if (comment == null) return NotFound("Comment Id not found");

            return Ok(comment);
        }

    }
}
