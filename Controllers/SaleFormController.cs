using InventSales.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace InventSales.Controllers
{
    public class SaleFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Sale sale)
        {
            List<Sale> sales = new List<Sale>();
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Invent;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Sales", connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Sale item = new Sale
                    {
                        ProductId = (int)dr["ProductId"],
                        StoreId = (int)dr["StoreId"],
                        Date = (DateTime)dr["Date"],
                        SalesQuantity = (int)dr["SalesQuantity"],
                        Stock = (int)dr["Stock"],
                    };

                    sales.Add(item);
                }
            }

            return View(sales);
        }
    }
}
