
using api.Dtos.Comments;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class CommentMapper : Profile
    {
        public CommentMapper()
        {
            CreateMap<Comments, CommentDto>();
            CreateMap<CreateCommentRequestDto, Comments>();
        }
    }
}
