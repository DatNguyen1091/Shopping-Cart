using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<OrderItems> GetAllOrderItems()
        {
            List<OrderItems> orderItems = new List<OrderItems>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM OrderItems", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderItems model = new OrderItems();
                            model.id = (int)reader["id"];
                            model.quantity = (int)reader["quantity"];
                            model.price = (decimal)reader["price"];
                            model.orderId = (int)reader["orderId"];
                            model.productId = (int)reader["productId"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            orderItems.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return orderItems;
        }

        [HttpGet("{id}")]
        public OrderItems GetOrderItemId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM OrderItems WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        OrderItems item = new OrderItems
                        {
                            id = reader.GetInt32(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            productId = reader.GetInt32(3),
                            orderId = reader.GetInt32(4),
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
        public OrderItems AddOrderItem(OrderItems model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO OrderItems (quantity, price, orderId, productId, createdAt, updatedAt) VALUES (@quantity, @price, @orderId, @productId, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@quantity", model.quantity);
                        command.Parameters.AddWithValue("@price", model.price);
                        command.Parameters.AddWithValue("@orderId", model.orderId);
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
        public OrderItems UpdateOrderItem(int id, OrderItems model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE OrderItems SET quantity = @quantity, price = @price, orderId = @orderId, productId = @productId, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
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
                            command.Parameters.AddWithValue("@price", model.price);
                            command.Parameters.AddWithValue("@orderId", model.orderId);
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
        public string RemoveOrderItem(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM OrderItems WHERE id = @id", connection))
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
