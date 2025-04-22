

namespace api.Dtos.Stocks
{
    public class StockResponse
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public List<StockDto> Stocks { get; set; } = [];

    }
}
