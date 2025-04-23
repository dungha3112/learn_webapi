
using api.Constants;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route(RouteConstants.COMMENTS)]
    [ApiController]
    [Authorize]
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
            var appUserId = User.GetUserId();

            var stockExists = await _stockRepository.StockExistsAsync(stockId);
            if (!stockExists) return BadRequest("Stock Id does not exists.");

            var commentDto = await _commentRepository.CreateCommentByStockIdAsync(stockId, commentRequestDto, appUserId);


            return CreatedAtAction(nameof(GetById), new { id = commentDto.Id }, commentDto);
        }

        [HttpPut("{id:int}")]
        //PUT /api/comments/stocks/{stockId}
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentRequestDto)
        {
            var appUserId = User.GetUserId();

            var commentDto = await _commentRepository.UpdateCommentByIdAsync(id, commentRequestDto, appUserId);
            if (commentDto == null) return NotFound("Stock Id not found");

            return Ok(commentDto);
        }


        [HttpDelete("{id:int}")]
        //PUT /api/comments/id
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var appUserId = User.GetUserId();

            var comment = await _commentRepository.DeleteCommentByIdAsync(id, appUserId);
            if (comment == null) return NotFound("Comment Id not found");

            return Ok(comment);
        }

    }
}
