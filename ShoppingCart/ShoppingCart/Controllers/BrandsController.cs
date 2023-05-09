using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;
using X.PagedList;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {      
        [HttpGet]
        public List<Brands> GetBrands(int? page)
        {
            List<Brands> brands = new List<Brands>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {   
                connection.Open();
                var offset = (pageIndex - 1) * pageSize;
                using (SqlCommand command = new SqlCommand("SELECT * FROM Brands ORDER BY id OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY", connection))
                {
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
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
            var brand = brands.ToPagedList(pageIndex, pageSize);
            return brand.ToList();
        }

        [HttpGet("{id}")]
        public Brands GetProductBrandId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO Brands ( name, slug, description, metaDescription, metaKeywords, brandStatus, isDeleted) VALUES ( @name, @slug, @description, @metaDescription, @metaKeywords, @brandStatus, @isDelete)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@slug", model.slug);
                        command.Parameters.AddWithValue("@description", model.description);
                        command.Parameters.AddWithValue("@metaDescription", model.metaDescription);
                        command.Parameters.AddWithValue("@metaKeywords", model.metaKeywords);
                        command.Parameters.AddWithValue("@brandStatus", model.brandStatus);
                        command.Parameters.AddWithValue("@isDelete", model.isDelete);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return model;
            }
        }

        [HttpPut("{id}")]
        public Brands UpdateBrands(int id, Brands model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE Brands SET name = @name, slug = @slug, description = @description, metaDescription = @metaDescription, metaKeywords = @metaKeywords, brandStatus = @brandStatus, isDeleted = @isDelete WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", model.name);
                    command.Parameters.AddWithValue("@slug", model.slug);
                    command.Parameters.AddWithValue("@description", model.description);
                    command.Parameters.AddWithValue("@metaDescription", model.metaDescription);
                    command.Parameters.AddWithValue("@metaKeywords", model.metaKeywords);
                    command.Parameters.AddWithValue("@brandStatus", model.brandStatus);
                    command.Parameters.AddWithValue("@isDelete", model.isDelete);
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
        public string RemoveBrands(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Brands WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Brand deleted successfully.";
                    }
                }
                connection.Close();
            }
            return "Failed to delete brand.";
        }
    }
}
