using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using System.Data.SqlClient;
using X.PagedList;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {

        [HttpGet]
        public List<OrderItems> GetOrderItems(int? page)
        {
            List<OrderItems> orderItems = new List<OrderItems>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var offset = (pageIndex - 1) * pageSize;
                using (SqlCommand command = new SqlCommand("SELECT * FROM OrderItems ORDER BY id OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY", connection))
                {
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
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
            var orderItem = orderItems.ToPagedList(pageIndex, pageSize);
            return orderItem.ToList();
        }

        [HttpGet("{id}")]
        public OrderItems GetOrderItemId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO OrderItems (quantity, price, orderId, productId) VALUES (@quantity, @price, @orderId, @productId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@quantity", model.quantity);
                    command.Parameters.AddWithValue("@price", model.price);
                    command.Parameters.AddWithValue("@orderId", model.orderId);
                    command.Parameters.AddWithValue("@productId", model.productId);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public OrderItems UpdateOrderItem(int id, OrderItems model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE OrderItems SET quantity = @quantity, price = @price, orderId = @orderId, productId = @productId WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@quantity", model.quantity);
                    command.Parameters.AddWithValue("@price", model.price);
                    command.Parameters.AddWithValue("@orderId", model.orderId);
                    command.Parameters.AddWithValue("@productId", model.productId);
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
        public string RemoveOrderItem(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
