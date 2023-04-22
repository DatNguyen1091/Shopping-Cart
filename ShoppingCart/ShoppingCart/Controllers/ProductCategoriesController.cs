using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<ProductCategories> GetAllProductCategories()
        {
            List<ProductCategories> productCategories = new List<ProductCategories>();
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            return productCategories;
        }

        [HttpGet("{id}")]
        public ProductCategories GetProductCategoryId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO ProductCategories  ( productId, categoryId, createdAt, updatedAt) VALUES ( @productId, @categoryId, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@productId", model.productId);
                        command.Parameters.AddWithValue("@categoryId", model.categoryId);
                        command.Parameters.AddWithValue("@createdAt", model.createdAt);
                        command.Parameters.AddWithValue("@updatedAt", model.updatedAt);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    connection.Close();
                    return model;
                }
                catch
                {
                    transaction.Rollback();
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPut("{id}")]
        public ProductCategories UpdateProductCategories(int id, ProductCategories model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE ProductCategories SET productId = @productId, categoryId = @categoryId, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@productId", model.productId);
                            command.Parameters.AddWithValue("@categoryId", model.categoryId);
                            command.Parameters.AddWithValue("@createdAt", model.createdAt);
                            command.Parameters.AddWithValue("@updatedAt", model.updatedAt);
                            int rows = command.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                transaction.Rollback();
                                return null!;
                            }
                        }
                        transaction.Commit();
                        return model;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null!;
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public string RemoveProductCategories(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM ProductCategories WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Item deleted successfully.";
                    }
                }
                connection.Close();
            }
            return "Failed to delete item.";
        }
    }
}
