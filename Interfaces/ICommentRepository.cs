
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {

        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<List<CommentDto>?> GetCommentsByStockIdAsync(int stockId);
        Task<CommentDto> CreateCommentByStockIdAsync(int stockId, CreateCommentRequestDto bodyCommentDto, string appUserId);
        Task<CommentDto?> UpdateCommentByIdAsync(int id, UpdateCommentRequestDto bodyCommentDto, string appUserId);
        Task<Comments?> DeleteCommentByIdAsync(int id, string appUserId);
    }
}
