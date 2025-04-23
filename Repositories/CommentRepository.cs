
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
            var comment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return null;

            var commentDto = _mapper.Map<CommentDto>(comment);

            return commentDto;
        }

        // create comment
        public async Task<CommentDto> CreateCommentByStockIdAsync(int stockId, CreateCommentRequestDto bodyCommentDto, string appUserId)
        {
            if (appUserId == null) throw new UnauthorizedAccessException("Unauthorized");

            var commentModel = _mapper.Map<Comments>(bodyCommentDto);
            commentModel.StockId = stockId;
            commentModel.AppUserId = appUserId;

            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentDto>(commentModel);

            return commentDto;
        }

        public async Task<CommentDto?> UpdateCommentByIdAsync(int id, UpdateCommentRequestDto bodyCommentDto, string appUserId)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null) return null;

            if (existingComment.AppUserId != appUserId) throw new UnauthorizedAccessException("You can not update comment.");

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
                .Include(c => c.AppUser)
                .ToListAsync();

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            return commentDtos;
        }

        public async Task<Comments?> DeleteCommentByIdAsync(int id, string appUserId)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return null;

            if (comment.AppUserId != appUserId) throw new UnauthorizedAccessException("You can not delete comment.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
