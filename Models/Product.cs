using System.ComponentModel.DataAnnotations;

namespace InventSales.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Cost { get; set; }

        public int SalesPrice { get; set; }
    }
}
