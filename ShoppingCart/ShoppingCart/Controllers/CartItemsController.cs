using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {

        [HttpGet]
        public List<CartItems> GetCartItems(int page)
        {
            int pageSize = 10;
            List<CartItems> cartItems = new List<CartItems>();
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                int startIndex = (page - 1) * pageSize + 1;
                int endIndex = page * pageSize;
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CartItems WHERE id BETWEEN @StartIndex AND @EndIndex ORDER BY id ASC;", connection))
                {
                    command.Parameters.AddWithValue("@StartIndex", startIndex);
                    command.Parameters.AddWithValue("@EndIndex", endIndex);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartItems model = new CartItems();
                            model.id = (int)reader["id"];
                            model.quantity = (int)reader["quantity"];
                            model.cartId = (int)reader["cartId"];
                            model.productId = (int)reader["productId"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            cartItems.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return cartItems;
        }

        [HttpGet("{id}")]
        public CartItems GetCartItemId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CartItems WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        CartItems item = new CartItems
                        {
                            id = reader.GetInt32(0),
                            quantity = reader.GetInt32(1),
                            cartId = reader.GetInt32(2),
                            productId = reader.GetInt32(3),
                            isDeleted = reader.GetBoolean(4),
                            createdAt = reader.GetDateTime(5),
                            updatedAt = reader.GetDateTime(6),
                        };
                        return item;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public CartItems AddCartItem(CartItems model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO CartItems (quantity, cartId, productId, isDeleted, createdAt) VALUES (@quantity, @cartId, @productId, @isDeleted, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@quantity", model.quantity);
                    command.Parameters.AddWithValue("@cartId", model.cartId);
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public CartItems UpdateCartItem(int id, CartItems model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE CartItems SET quantity = @quantity, cartId = @cartId, productId = @productId, isDeleted = @isDeleted, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@quantity", model.quantity);
                    command.Parameters.AddWithValue("@cartId", model.cartId);
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
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
        public string RemoveCartItem(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM CartItems WHERE id = @id", connection))
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
