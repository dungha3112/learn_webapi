
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stocks
{
    public class UpdateStockRequestDto
    {
        [Required(ErrorMessage = "The Symbol is required")]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Company Name is required")]
        [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Purchase is required")]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required(ErrorMessage = "The LastDiv is required")]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required(ErrorMessage = "The Industry is required")]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
        public string Industry { get; set; } = string.Empty;

        [Required(ErrorMessage = "The MarketCap is required")]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}
