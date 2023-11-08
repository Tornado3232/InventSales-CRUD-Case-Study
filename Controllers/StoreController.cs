using Humanizer;
using InventSales.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace InventSales.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Invent;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Store> stores = new List<Store>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Stores", connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Store item = new Store
                    {
                        Id = (int)dr["Id"],
                        StoreName = dr["StoreName"].ToString(),
                    };
                    stores.Add(item);
                }
            }
            return View(stores);
        }

        public IActionResult GetProfit(Store store)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Sales.StoreId AS Id,Stores.StoreName AS StoreName, SUM(Sales.SalesQuantity*(Products.SalesPrice-Products.Cost)) AS Profit "+
                                                    "FROM Sales INNER JOIN Products ON Sales.ProductId = Products.Id "+
                                                    "INNER JOIN Stores ON Sales.StoreId=Stores.Id "+
                                                    "WHERE Sales.StoreId = @storeId "+
                                                    "GROUP BY Sales.StoreId, Stores.StoreName", connection);
                command.Connection.Open();
                command.Parameters.Add(new SqlParameter("@storeId", store.Id));
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Store item = new Store
                    {
                        Id = (int)dr["Id"],
                        StoreName= dr["StoreName"].ToString(),
                        Profit = (int)dr["Profit"]
                    };
                    return View(item);
                }
                return BadRequest();
            }
        }

        public IActionResult GetMostProfit()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 Sales.StoreId AS Id,Stores.StoreName AS StoreName, SUM(Sales.SalesQuantity*(Products.SalesPrice-Products.Cost)) AS Profit " +
                                                    "FROM Sales INNER JOIN Products ON Sales.ProductId = Products.Id " +
                                                    "INNER JOIN Stores ON Sales.StoreId=Stores.Id " +
                                                    "GROUP BY Sales.StoreId, Stores.StoreName "+
                                                    "ORDER BY Profit DESC", connection);
                command.Connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Store item = new Store
                    {
                        Id = (int)dr["Id"],
                        StoreName = dr["StoreName"].ToString(),
                        Profit = (int)dr["Profit"]
                    };
                    return View(item);
                }
                return BadRequest();
            }
        }

        public IActionResult Store()
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
