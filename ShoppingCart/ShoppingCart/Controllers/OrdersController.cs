using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Orders", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order model = new Order();
                            model.id = (int)reader["id"];
                            model.orderTotal = (decimal)reader["orderTotal"];
                            model.orderItemTotal = (decimal)reader["orderItemTotal"];
                            model.shippingCharge = (decimal)reader["shippingCharge"];
                            model.deliveryAddressId = (int)reader["deliveryAddressId"];
                            model.customerId = (int)reader["customerId"];
                            model.orderStatus = (string)reader["orderStatus"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            orders.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return orders;
        }

        [HttpGet("{id}")]
        public Order GetOrderId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Order model = new Order
                        {
                            id = reader.GetInt32(0),
                            orderTotal = reader.GetDecimal(1),
                            orderItemTotal = reader.GetDecimal(2),
                            shippingCharge = reader.GetDecimal(3),
                            deliveryAddressId = reader.GetInt32(4),
                            customerId = reader.GetInt32(5),
                            orderStatus = reader.GetString(6),
                            isDeleted = reader.GetBoolean(7),
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
        public Order AddOrder(Order model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO Orders ( orderTotal, orderItemTotal, shippingCharge, deliveryAddressId, customerId, orderStatus, isDeleted, createdAt, updatedAt) VALUES ( @orderTotal, @orderItemTotal, @shippingCharge, @deliveryAddressId, @customerId, @orderStatus, @isDeleted, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@orderTotal", model.orderTotal);
                        command.Parameters.AddWithValue("@orderItemTotal", model.orderItemTotal);
                        command.Parameters.AddWithValue("@shippingCharge", model.shippingCharge);
                        command.Parameters.AddWithValue("@deliveryAddressId", model.deliveryAddressId);
                        command.Parameters.AddWithValue("@customerId", model.customerId);
                        command.Parameters.AddWithValue("@orderStatus", model.orderStatus);
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
        public Order UpdateOrder(int id, Order model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Orders SET orderTotal = @orderTotal, orderItemTotal = @orderItemTotal, shippingCharge = @shippingCharge, deliveryAddressId = @deliveryAddressId, customerId = @customerId, orderStatus = @orderStatus, isDeleted = @isDeleted, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@orderTotal", model.orderTotal);
                            command.Parameters.AddWithValue("@orderItemTotal", model.orderItemTotal);
                            command.Parameters.AddWithValue("@shippingCharge", model.shippingCharge);
                            command.Parameters.AddWithValue("@deliveryAddressId", model.deliveryAddressId);
                            command.Parameters.AddWithValue("@customerId", model.customerId);
                            command.Parameters.AddWithValue("@orderStatus", model.orderStatus);
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
        public string RemoveOrder(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE id = @id", connection))
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
