
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public StockRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /*
            get all
        */
        public async Task<StockResponse> GetAllAsync(StockQueryObject query)
        {

            var totalItems = await _context.Stocks.CountAsync();
            var totalPage = (int)Math.Ceiling(totalItems / (double)query.PageSize);

            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            // Sorting
            // if (!string.IsNullOrWhiteSpace(query.SortBy))
            // {
            //     if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            //     {
            //         stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            //     }
            //     else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
            //     {
            //         stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            //     }
            // }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = query.SortBy.ToLower() switch
                {
                    "symbol" => query.IsDecsending
                        ? stocks.OrderByDescending(s => s.Symbol)
                        : stocks.OrderBy(s => s.Symbol),

                    "companyname" => query.IsDecsending
                        ? stocks.OrderByDescending(s => s.CompanyName)
                        : stocks.OrderBy(s => s.CompanyName),

                    _ => stocks
                };
            }

            // Pagination
            var skip = (query.PageNumber - 1) * query.PageSize;


            var result = await stocks.Skip(skip).Take(query.PageSize).ToListAsync();
            var stockDtos = _mapper.Map<List<StockDto>>(result);

            var response = new StockResponse
            {
                CurrentPage = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPage = totalPage,
                Stocks = stockDtos
            };
            return response;
        }

        /**
         GetByIdAsync
        */
        public async Task<StockDto?> GetByIdAsync(int id)
        {
            var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<StockDto>(stock);
        }

        /**
         CreateAsync
        */
        public async Task<StockDto> CreateAsync(CreateStockRequestDto createStockDto)
        {
            var stockModel = _mapper.Map<Stocks>(createStockDto);
            await _context.Stocks.AddAsync(stockModel);

            await _context.SaveChangesAsync();
            var stockDto = _mapper.Map<StockDto>(stockModel);
            return stockDto;
        }

        /**
        UpdateByIdAsync
        */
        public async Task<StockDto?> UpdateByIdAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null) return null;

            _mapper.Map(stockDto, stock);

            await _context.SaveChangesAsync();

            return _mapper.Map<StockDto>(stock);
        }

        /**
        DeleteByIdAsync
        */
        public async Task<Stocks?> DeleteByIdAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null) return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}
