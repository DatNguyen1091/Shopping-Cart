using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {      
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<Brands> GetAllBrands()
        {
            List<Brands> brands = new List<Brands>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Brands", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Brands model = new Brands();
                            model.id = (int)reader["id"];
                            model.name = (string)reader["name"];
                            model.slug = (string)reader["slug"];
                            model.description = (string)reader["description"];
                            model.metaDescription = (string)reader["metaDescription"];
                            model.metaKeywords = (string)reader["metaKeywords"];
                            model.brandStatus = (string)reader["brandStatus"];
                            model.isDelete = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            brands.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return brands;
        }

        [HttpGet("{id}")]
        public Brands GetProductBrandId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Brands WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Brands model = new Brands
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            slug = reader.GetString(2),
                            description = reader.GetString(3),
                            metaDescription = reader.GetString(4),
                            metaKeywords = reader.GetString(5),
                            brandStatus = reader.GetString(6),
                            isDelete = reader.GetBoolean(7),
                            createdAt = reader.GetDateTime(8),
                            updatedAt = reader.GetDateTime(9),
                        };
                        return model;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public Brands AddProductCategories(Brands model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO Brands ( name, slug, description, metaDescription, metaKeywords, brandStatus, isDeleted, createdAt, updatedAt) VALUES ( @name, @slug, @description, @metaDescription, @metaKeywords, @brandStatus, @isDelete, @createdAt, @updatedAt)";
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
                        command.Parameters.AddWithValue("@brandStatus", model.brandStatus);
                        command.Parameters.AddWithValue("@isDelete", model.isDelete);
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
        public Brands UpdateBrands(int id, Brands model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Brands SET name = @name, slug = @slug, description = @description, metaDescription = @metaDescription, metaKeywords = @metaKeywords, brandStatus = @brandStatus, isDeleted = @isDelete, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
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
                            command.Parameters.AddWithValue("@brandStatus", model.brandStatus);
                            command.Parameters.AddWithValue("@isDelete", model.isDelete);
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
        public string RemoveBrands(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Brands WHERE id = @id", connection))
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
