﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<Addresses> GetAllAddresses()
        {
            List<Addresses> addresses = new List<Addresses>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Addresses", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Addresses model = new Addresses();
                            model.id = (int)reader["id"];
                            model.name = (string)reader["name"];
                            model.addressLine1 = (string)reader["addressLine1"];
                            model.addressLine2 = (string)reader["addressLine2"];
                            model.city = (string)reader["city"];
                            model.state = (string)reader["state"];
                            model.country = (string)reader["country"];
                            model.zipCode = (string)reader["zipCode"];
                            model.addressType = (string)reader["addressType"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            addresses.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return addresses;
        }

        [HttpGet("{id}")]
        public Addresses GetAddressId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Addresses WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Addresses model = new Addresses
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            addressLine1 = reader.GetString(2),
                            addressLine2 = reader.GetString(3),
                            city = reader.GetString(4),
                            state = reader.GetString(5),
                            country = reader.GetString(6),
                            zipCode = reader.GetString(7),
                            addressType = reader.GetString(8),
                            isDeleted = reader.GetBoolean(9),
                            createdAt = reader.GetDateTime(10),
                            updatedAt = reader.GetDateTime(11),
                        };
                        return model;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public Addresses AddAddress(Addresses model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO Addresses ( name, addressLine1, addressLine2, city, state, country, zipCode, addressType, isDeleted, createdAt, updatedAt) VALUES ( @name, @addressLine1, @addressLine2, @city, @state, @country, @zipCode, @addressType, @isDeleted, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@name", model.name);
                        command.Parameters.AddWithValue("@addressLine1", model.addressLine1);
                        command.Parameters.AddWithValue("@addressLine2", model.addressLine2);
                        command.Parameters.AddWithValue("@city", model.city);
                        command.Parameters.AddWithValue("@state", model.state);
                        command.Parameters.AddWithValue("@country", model.country);
                        command.Parameters.AddWithValue("@zipCode", model.zipCode);
                        command.Parameters.AddWithValue("@addressType", model.addressType);
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
        public Addresses UpdateAddress(int id, Addresses model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE Addresses SET name = @name, addressLine1 = @addressLine1, addressLine2 = @addressLine2, city = @city, state = @state, country = @country, zipCode = @zipCode, addressType = @addressType, isDeleted = @isDeleted, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
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
                            command.Parameters.AddWithValue("@addressLine1", model.addressLine1);
                            command.Parameters.AddWithValue("@addressLine2", model.addressLine2);
                            command.Parameters.AddWithValue("@city", model.city);
                            command.Parameters.AddWithValue("@state", model.state);
                            command.Parameters.AddWithValue("@country", model.country);
                            command.Parameters.AddWithValue("@zipCode", model.zipCode);
                            command.Parameters.AddWithValue("@addressType", model.addressType);
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
        public string RemoveAddress(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Addresses WHERE id = @id", connection))
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
