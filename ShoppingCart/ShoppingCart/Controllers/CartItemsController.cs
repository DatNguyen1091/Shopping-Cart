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
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<CartItems> GetAllCartItems()
        {
            List<CartItems> cartItems = new List<CartItems>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CartItems", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartItems model = new CartItems();
                            model.id = (int)reader["id"];
                            model.quantity = (int)reader["quantity"];
                            model.cartId = (int)reader["cartId"];
                            model.productId = (int)reader["productId"];
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
            using (SqlConnection connection = new SqlConnection(connectionString))
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
                            createdAt = reader.GetDateTime(4),
                            updatedAt = reader.GetDateTime(5),
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO CartItems (quantity, cartId, productId, createdAt, updatedAt) VALUES (@quantity, @cartId, @productId, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@quantity", model.quantity);
                        command.Parameters.AddWithValue("@cartId", model.cartId);
                        command.Parameters.AddWithValue("@productId", model.productId);
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
        public CartItems UpdateCartItem(int id, CartItems model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE CartItems SET quantity = @quantity, cartId = @cartId, productId = @productId, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@quantity", model.quantity);
                            command.Parameters.AddWithValue("@cartId", model.cartId);
                            command.Parameters.AddWithValue("@productId", model.productId);
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
        public string RemoveCartItem(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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
