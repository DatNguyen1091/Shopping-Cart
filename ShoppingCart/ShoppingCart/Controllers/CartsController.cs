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
        public List<Carts> GetCarts(int start, int limit)
        {
            List<Carts> carts = new List<Carts>();
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Carts ORDER BY id OFFSET (@start-1) ROWS FETCH NEXT @limit ROWS ONLY;", connection))
                {
                    command.Parameters.AddWithValue("@start", start);
                    command.Parameters.AddWithValue("@limit", limit);

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
                }
                connection.Close();
            }
            return carts;
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
                var query = "INSERT INTO Carts (uniqueCartId, cartStatus) VALUES (@uniqueCartId, @cartStatus)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uniqueCartId", model.uniqueCartId);
                    command.Parameters.AddWithValue("@cartStatus", model.cartStatus);
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
                var query = "UPDATE Carts SET uniqueCartId = @uniqueCartId, cartStatus = @cartStatus WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@uniqueCartId", model.uniqueCartId);
                    command.Parameters.AddWithValue("@cartStatus", model.cartStatus);
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
