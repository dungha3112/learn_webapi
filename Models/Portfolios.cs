

using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolios
    {
        public string AppUserId { get; set; } = string.Empty;
        public int StockId { get; set; }
        public AppUser AppUser { get; set; } = new AppUser();

        public Stocks Stock { get; set; } = new Stocks();

    }
}
