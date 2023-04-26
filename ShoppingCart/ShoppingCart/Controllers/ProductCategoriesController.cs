using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;
using System.Transactions;
using X.PagedList;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    { 

        [HttpGet]
        public List<ProductCategories> GetProductCategories(int? page)
        {
            List<ProductCategories> productCategories = new List<ProductCategories>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM ProductCategories", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductCategories model = new ProductCategories();
                            model.id = (int)reader["id"];
                            model.productId = (int)reader["productId"];
                            model.categoryId = (int)reader["categoryId"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            productCategories.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            var productCategory = productCategories.ToPagedList(pageIndex, pageSize);
            return productCategory.ToList();
        }

        [HttpGet("{id}")]
        public ProductCategories GetProductCategoryId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM ProductCategories WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ProductCategories model = new ProductCategories
                        {
                            id = reader.GetInt32(0),
                            productId = reader.GetInt32(1),
                            categoryId = reader.GetInt32(2),
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
        public ProductCategories AddProductCategories(ProductCategories model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO ProductCategories  ( productId, categoryId, createdAt) VALUES ( @productId, @categoryId, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@categoryId", model.categoryId);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public ProductCategories UpdateProductCategories(int id, ProductCategories model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE ProductCategories SET productId = @productId, categoryId = @categoryId, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@categoryId", model.categoryId);
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
        public string RemoveProductCategories(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM ProductCategories WHERE id = @id", connection))
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
