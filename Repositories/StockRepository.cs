
using api.Data;
using api.Dtos.Stock;
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
        public async Task<StockResponse> GetAllAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 3;
            var totalItems = _context.Stocks.Count();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);


            var stocks = await _context.Stocks
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Include(c => c.Comments)
                        .ToListAsync();
            var stockDtos = _mapper.Map<List<StockDto>>(stocks);

            var response = new StockResponse
            {
                CurrentPage = page,
                PageSize = pageSize,
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
