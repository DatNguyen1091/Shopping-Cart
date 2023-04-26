using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using X.PagedList;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandsController : ControllerBase
    {

        [HttpGet]
        public List<ProductBrands> GetProductCategories(int? page)
        {
            List<ProductBrands> productBrands = new List<ProductBrands>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM ProductBrands", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductBrands model = new ProductBrands();
                            model.id = (int)reader["id"];
                            model.productId = (int)reader["productId"];
                            model.brandId = (int)reader["brandId"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            productBrands.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            var productBrand = productBrands.ToPagedList(pageIndex, pageSize);
            return productBrand.ToList();
        }

        [HttpGet("{id}")]
        public ProductBrands GetProductBrandId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM ProductBrands WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ProductBrands model = new ProductBrands
                        {
                            id = reader.GetInt32(0),
                            productId = reader.GetInt32(1),
                            brandId = reader.GetInt32(2),
                            createdAt = reader.GetDateTime(3),
                            updatedAt = reader.GetDateTime(4),
                        };
                        return model;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public ProductBrands AddProductBrands(ProductBrands model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO ProductBrands  ( productId, brandId, createdAt) VALUES ( @productId, @brandId, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@brandId", model.brandId);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public ProductBrands UpdateProductBrand(int id, ProductBrands model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE ProductBrands SET productId = @productId, brandId = @brandId, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@brandId", model.brandId);
                    command.Parameters.AddWithValue("@updatedAt", model.updatedAt);
                    int rows = command.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        return null!;
                    }
                }
                return model;
            }
        }

        [HttpDelete("{id}")]
        public string RemoveProductBrands(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM ProductBrands WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Deleted successfully.";
                    }
                }
                connection.Close();
            }
            return "Failed to delete.";
        }
    }
}
