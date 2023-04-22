using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<Products> GetAllProducts()
        {
            List<Products> products = new List<Products>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Products", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Products model = new Products();
                            model.id = (int)reader["id"];
                            model.name = (string)reader["name"];
                            model.slug = (string)reader["slug"];
                            model.description = (string)reader["description"];
                            model.metaDescription = (string)reader["metaDescription"];
                            model.metaKeywords = (string)reader["metaKeywords"];
                            model.sku = (string)reader["sku"];
                            model.model = (string)reader["model"];
                            model.price = (decimal)reader["price"];
                            model.oldPrice = (decimal)reader["oldPrice"];
                            model.imageUrl = (string)reader["imageUrl"];
                            model.isBestseller = (bool)reader["isBestseller"];
                            model.isFeatured = (bool)reader["isFeatured"];
                            model.quantity = (int)reader["quantity"];
                            model.productStatus = (string)reader["productStatus"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            products.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return products;
        }

        [HttpGet("{id}")]
        public Products GetProductsId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Products item = new Products
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            slug = reader.GetString(2),
                            description = reader.GetString(3),
                            metaDescription = reader.GetString(4),
                            metaKeywords = reader.GetString(5),
                            sku = reader.GetString(6),
                            model = reader.GetString(7),
                            price = reader.GetDecimal(8),
                            oldPrice = reader.GetDecimal(9),
                            imageUrl = reader.GetString(10),
                            isBestseller = reader.GetBoolean(11),
                            isFeatured = reader.GetBoolean(12),
                            quantity = reader.GetInt32(13),
                            productStatus = reader.GetString(14),
                            isDeleted = reader.GetBoolean(15),
                            createdAt = reader.GetDateTime(16),
                            updatedAt = reader.GetDateTime(17),
                        };
                        return item;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public Products AddProduct(Products model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO Products (name, slug, description, metaDescription, metaKeywords, sku, model, price, oldPrice, imageUrl, isBestseller, isFeatured, quantity, productStatus, isDeleted, createdAt, updatedAt) VALUES (@name, @slug, @description, @metaDescription, @metaKeywords, @sku, @model, @price, @oldPrice, @imageUrl, @isBestseller, @isFeatured, @quantity, @productStatus, @isDeleted, @createdAt, @updatedAt)";
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
                        command.Parameters.AddWithValue("@sku", model.sku);
                        command.Parameters.AddWithValue("@model", model.model);
                        command.Parameters.AddWithValue("@price", model.price);
                        command.Parameters.AddWithValue("@oldPrice", model.oldPrice);
                        command.Parameters.AddWithValue("@quantity", model.quantity);
                        command.Parameters.AddWithValue("@imageUrl", model.imageUrl);
                        command.Parameters.AddWithValue("@isBestseller", model.isBestseller);
                        command.Parameters.AddWithValue("@isFeatured", model.isFeatured);
                        command.Parameters.AddWithValue("@productStatus", model.productStatus);
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
        public Products UpdateProduct(int id, Products model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Products SET name = @name, slug = @slug, description = @description, metaDescription = @metaDescription, metaKeywords = @metaKeywords, sku = @sku, model = @model, price = @price, oldPrice = @oldPrice, imageUrl = @imageUrl, isBestseller = @isBestseller, isFeatured = @isFeatured, quantity = @quantity, productStatus = @productStatus, isDeleted = @isDeleted, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
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
                            command.Parameters.AddWithValue("@sku", model.sku);
                            command.Parameters.AddWithValue("@model", model.model);
                            command.Parameters.AddWithValue("@price", model.price);
                            command.Parameters.AddWithValue("@oldPrice", model.oldPrice);
                            command.Parameters.AddWithValue("@quantity", model.quantity);
                            command.Parameters.AddWithValue("@imageUrl", model.imageUrl);
                            command.Parameters.AddWithValue("@isBestseller", model.isBestseller);
                            command.Parameters.AddWithValue("@isFeatured", model.isFeatured);
                            command.Parameters.AddWithValue("@productStatus", model.productStatus);
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
        public string RemoveProduct(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Products WHERE id = @id", connection))
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
