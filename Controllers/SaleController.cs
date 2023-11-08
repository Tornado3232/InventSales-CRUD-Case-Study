using InventSales.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Linq;

namespace InventSales.Controllers
{
    public class SaleController : Controller
    {
        private readonly ILogger<SaleController> _logger;
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Invent;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public SaleController(ILogger<SaleController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Sale> sales = new List<Sale>();

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

        [HttpPost]
        public IActionResult Add(Sale sale)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Sales VALUES (@productId, @storeId, @date, @salesQuantity, @stock)", connection);
                command.Connection.Open();
                command.Parameters.Add(new SqlParameter("@productId", sale.ProductId));
                command.Parameters.Add(new SqlParameter("@storeId", sale.StoreId));
                command.Parameters.Add(new SqlParameter("@date", sale.Date));
                command.Parameters.Add(new SqlParameter("@salesQuantity", sale.SalesQuantity));
                command.Parameters.Add(new SqlParameter("@stock", sale.Stock));
                command.ExecuteNonQuery();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Find(Sale sale)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Sales WHERE ProductId=@productId AND StoreId=@storeId AND Date=@date", connection);
                command.Connection.Open();
                command.Parameters.Add(new SqlParameter("@productId", sale.ProductId));
                command.Parameters.Add(new SqlParameter("@storeId", sale.StoreId));
                command.Parameters.Add(new SqlParameter("@date", sale.Date));
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
                    return View(item);
                }
                return BadRequest();
            }
        }

        public IActionResult Update(Sale sale)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Sales SET SalesQuantity=@salesQuantity, Stock=@stock WHERE ProductId=@productId AND StoreId=@storeId AND Date=@date", connection);
                command.Connection.Open();
                command.Parameters.Add(new SqlParameter("@productId", sale.ProductId));
                command.Parameters.Add(new SqlParameter("@storeId", sale.StoreId));
                command.Parameters.Add(new SqlParameter("@date", sale.Date));
                command.Parameters.Add(new SqlParameter("@salesQuantity", sale.SalesQuantity));
                command.Parameters.Add(new SqlParameter("@stock", sale.Stock));
                command.ExecuteNonQuery();
            }
            return View();
        }

        public IActionResult Delete(Sale sale)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Sales WHERE ProductId=@productId AND StoreId=@storeId AND Date=@date", connection);
                command.Connection.Open();
                command.Parameters.Add(new SqlParameter("@productId", sale.ProductId));
                command.Parameters.Add(new SqlParameter("@storeId", sale.StoreId));
                command.Parameters.Add(new SqlParameter("@date", sale.Date));
                command.ExecuteNonQuery();
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
