using System;
using System.Data.SqlClient;
using InventSales.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventSales.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Invent;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Products", connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Product item = new Product
                    {
                        Id = (int)dr["Id"],
                        ProductName = dr["ProductName"].ToString(),
                        Cost = (int)dr["Cost"],
                        SalesPrice = (int)dr["SalesPrice"]
                    };
                    products.Add(item);
                }
            }

            return View(products);
        }

        public IActionResult GetBestSeller()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 Sales.ProductId AS Id, Products.ProductName AS ProductName, SUM(Sales.SalesQuantity) AS Quantity " +
                                                    "FROM Sales INNER JOIN Products ON Sales.ProductId = Products.Id " +
                                                    "GROUP BY Sales.ProductId, Products.ProductName " +
                                                    "ORDER BY Quantity DESC", connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    ProductVM item = new ProductVM
                    {
                        product = new Models.Product { Id = (int)dr["Id"], ProductName=dr["ProductName"].ToString() },
                        quantity = (int)dr["Quantity"],
                    };
                    return View(item);
                }
                return BadRequest();
            }
        }

        public IActionResult Product()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
