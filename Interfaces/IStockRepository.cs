
using api.Dtos.Stocks;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<StockResponse> GetAllAsync(StockQueryObject query);
        Task<StockDto?> GetByIdAsync(int id);
        Task<StockDto> CreateAsync(CreateStockRequestDto stockModel);
        Task<StockDto?> UpdateByIdAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stocks?> DeleteByIdAsync(int id);

        Task<bool> StockExistsAsync(int id);
    }
}
