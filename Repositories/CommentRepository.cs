
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Get comment by id
        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return null;

            var commentDto = _mapper.Map<CommentDto>(comment);

            return commentDto;
        }

        // create comment
        public async Task<CommentDto> CreateCommentByStockIdAsync(int stockId, CreateCommentRequestDto bodyCommentDto)
        {
            var commentModel = _mapper.Map<Comments>(bodyCommentDto);
            commentModel.StockId = stockId;

            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentDto>(commentModel);

            return commentDto;
        }

        public async Task<CommentDto?> UpdateCommentByStockIdAsync(int stockId, UpdateCommentRequestDto bodyCommentDto)
        {
            var existingComment = await _context.Comments.FindAsync(stockId);
            if (existingComment == null) return null;

            existingComment.Content = bodyCommentDto.Content;
            existingComment.Title = bodyCommentDto.Title;

            await _context.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentDto>(existingComment);

            return commentDto;
        }

        public async Task<List<CommentDto>?> GetCommentsByStockIdAsync(int stockId)
        {

            var comments = await _context.Comments
                .Where(c => c.StockId == stockId)
                // .OrderByDescending(c => c.CreatedOn)
                // .Skip(1)
                // .Take(3)
                .ToListAsync();

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            return commentDtos;
        }

        public async Task<Comments?> DeleteCommentByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return null;


            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
