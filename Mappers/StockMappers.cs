
using api.Dtos.Stock;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class StockMappers : Profile
    {
        public StockMappers()
        {
            CreateMap<Stocks, StockDto>().ForMember(dest => dest.Comments, otp => otp.MapFrom(src => src.Comments));
            CreateMap<CreateStockRequestDto, Stocks>();

            CreateMap<UpdateStockRequestDto, Stocks>();
        }
    }
}
