
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {

        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<List<CommentDto>?> GetCommentsByStockIdAsync(int stockId);
        Task<CommentDto> CreateCommentByStockIdAsync(int stockId, CreateCommentRequestDto bodyCommentDto);
        Task<CommentDto?> UpdateCommentByStockIdAsync(int stockId, UpdateCommentRequestDto bodyCommentDto);
        Task<Comments?> DeleteCommentByIdAsync(int id);
    }
}
