using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<Categories> GetAllCategories()
        {
            List<Categories> categories = new List<Categories>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Categories", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categories model = new Categories();
                            model.id = (int)reader["id"];
                            model.name = (string)reader["name"];
                            model.slug = (string)reader["slug"];
                            model.description = (string)reader["description"];
                            model.metaDescription = (string)reader["metaDescription"];
                            model.metaKeywords = (string)reader["metaKeywords"];
                            model.categoryStatus = (string)reader["categoryStatus"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            categories.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return categories;
        }

        [HttpGet("{id}")]
        public Categories GetCategoriesId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Categories WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Categories item = new Categories
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            slug = reader.GetString(2),
                            description = reader.GetString(3),
                            metaDescription = reader.GetString(4),
                            metaKeywords = reader.GetString(5),
                            categoryStatus = reader.GetString(6),
                            isDeleted = reader.GetBoolean(7),
                            createdAt = reader.GetDateTime(8),
                            updatedAt = reader.GetDateTime(9),
                        };
                        return item;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public Categories AddCategories(Categories model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO Categories (name, slug, description, metaDescription, metaKeywords, categoryStatus,isDeleted, createdAt, updatedAt) VALUES (@name, @slug, @description, @metaDescription, @metaKeywords, @categoryStatus,@isDeleted, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@slug", model.slug);
                        command.Parameters.AddWithValue("@description", model.description);
                        command.Parameters.AddWithValue("@metaDescription", model.metaDescription);
                        command.Parameters.AddWithValue("@metaKeywords", model.metaKeywords);
                        command.Parameters.AddWithValue("@categoryStatus", model.categoryStatus);
                        command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
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
        public Categories UpdateCategory(int id, Categories model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Categories SET name = @name, slug = @slug, description = @description, metaDescription = @metaDescription, metaKeywords = @metaKeywords, categoryStatus = @categoryStatus, isDeleted = @isDeleted, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@name", model.name);
                            command.Parameters.AddWithValue("@slug", model.slug);
                            command.Parameters.AddWithValue("@description", model.description);
                            command.Parameters.AddWithValue("@metaDescription", model.metaDescription);
                            command.Parameters.AddWithValue("@metaKeywords", model.metaKeywords);
                            command.Parameters.AddWithValue("@categoryStatus", model.categoryStatus);
                            command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
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
        public string RemoveCategory(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Categories WHERE id = @id", connection))
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
