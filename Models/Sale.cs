using System.ComponentModel.DataAnnotations;

namespace InventSales.Models
{
    public class Sale
    {
        public int ProductId { get; set; }

        public int StoreId { get; set; }

        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        public int SalesQuantity { get; set; }

        public int Stock { get; set; }
    }
}
