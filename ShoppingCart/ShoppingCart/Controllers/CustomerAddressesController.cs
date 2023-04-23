using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<CustomerAddresses> GetAllCustomerAddresses()
        {
            List<CustomerAddresses> customerAddresses = new List<CustomerAddresses>();
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            return customerAddresses;
        }

        [HttpGet("{id}")]
        public CustomerAddresses GetCustomerAddressesId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO CustomerAddresses  ( customerId, addressId, createdAt, updatedAt) VALUES ( @customerId, @addressId, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@customerId", model.customerId);
                        command.Parameters.AddWithValue("@addressId", model.addressId);
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
        public CustomerAddresses UpdateCustomerAddresses(int id, CustomerAddresses model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE CustomerAddresses SET customerId = @customerId, addressId = @addressId, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@customerId", model.customerId);
                            command.Parameters.AddWithValue("@addressId", model.addressId);
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
        public string RemoveCustomerAddresses(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM CustomerAddresses WHERE id = @id", connection))
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
