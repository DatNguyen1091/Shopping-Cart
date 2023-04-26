using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        [HttpGet]
        public List<Carts> GetCarts(int page)
        {
            List<Carts> carts = new List<Carts>();
            int pageSize = 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Carts", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Carts model = new Carts();
                            model.id = (int)reader["id"];
                            model.uniqueCartId = (string)reader["uniqueCartId"];
                            model.cartStatus = (string)reader["cartStatus"];                          
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            carts.Add(model);
                        }
                    }
                    connection.Close();
                    var cart = carts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return cart;
                }
            }
        }

        [HttpGet("{id}")]
        public Carts GetCartsId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Carts WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Carts cart = new Carts
                        {
                            id = reader.GetInt32(0),
                            uniqueCartId = reader.GetString(1),
                            cartStatus = reader.GetString(2),
                            createdAt = reader.GetDateTime(3),
                            updatedAt = reader.GetDateTime(4),
                        };
                        return cart;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public Carts AddCart(Carts model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO Carts (uniqueCartId, cartStatus, createdAt) VALUES (@uniqueCartId, @cartStatus, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uniqueCartId", model.uniqueCartId);
                    command.Parameters.AddWithValue("@cartStatus", model.cartStatus);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public Carts UpdateCart(int id, Carts model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE Carts SET uniqueCartId = @uniqueCartId, cartStatus = @cartStatus, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@uniqueCartId", model.uniqueCartId);
                    command.Parameters.AddWithValue("@cartStatus", model.cartStatus);
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
        public string RemoveCart(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Carts WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Cart deleted successfully.";
                    }
                }
                connection.Close();
            }
            return "Failed to delete cart.";
        }
    }
}
