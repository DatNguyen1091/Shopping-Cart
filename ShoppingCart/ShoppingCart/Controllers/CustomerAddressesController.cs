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
    public class CustomerAddressesController : ControllerBase
    {

        [HttpGet]
        public List<CustomerAddresses> GetCustomerAddresses(int? page)
        {
            List<CustomerAddresses> customerAddresses = new List<CustomerAddresses>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CustomerAddresses", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerAddresses model = new CustomerAddresses();
                            model.id = (int)reader["id"];
                            model.customerId = (int)reader["customerId"];
                            model.addressId = (int)reader["addressId"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            customerAddresses.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            var customerAddress = customerAddresses.ToPagedList(pageIndex, pageSize);
            return customerAddress.ToList();
        }

        [HttpGet("{id}")]
        public CustomerAddresses GetCustomerAddressesId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM CustomerAddresses WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        CustomerAddresses model = new CustomerAddresses
                        {
                            id = reader.GetInt32(0),
                            customerId = reader.GetInt32(1),
                            addressId = reader.GetInt32(2),
                            createdAt = reader.GetDateTime(3),
                            updatedAt = reader.GetDateTime(4),
                        };
                        return model;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public CustomerAddresses AddCustomerAddresses(CustomerAddresses model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO CustomerAddresses  ( customerId, addressId, createdAt) VALUES ( @customerId, @addressId, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", model.customerId);
                    command.Parameters.AddWithValue("@addressId", model.addressId);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public CustomerAddresses UpdateCustomerAddresses(int id, CustomerAddresses model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE CustomerAddresses SET customerId = @customerId, addressId = @addressId, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@customerId", model.customerId);
                    command.Parameters.AddWithValue("@addressId", model.addressId);
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
        public string RemoveCustomerAddresses(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM CustomerAddresses WHERE id = @id", connection))
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
