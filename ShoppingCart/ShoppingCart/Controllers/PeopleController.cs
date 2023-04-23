using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly string connectionString = "Server=DATNGUYEN\\SQLEXPRESS;Database=ShoppingCart000;Integrated Security=True;";

        [HttpGet]
        public List<People> GetAllPeople()
        {
            List<People> peoples = new List<People>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM People", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            People model = new People();
                            model.id = (int)reader["id"];
                            model.firstName = (string)reader["firstName"];
                            model.middleName = (string)reader["middleName"];
                            model.lastName = (string)reader["lastName"];
                            model.emailAddress = (string)reader["emailAddress"];
                            model.phoneNumber = (string)reader["phoneNumber"];
                            model.gender = (string)reader["gender"];
                            model.dateOfBirth = (DateTime)reader["dateOfBirth"];
                            model.isDeleted = (bool)reader["isDeleted"];
                            model.createdAt = (DateTime)reader["createdAt"];
                            model.updatedAt = (DateTime)reader["updatedAt"];
                            peoples.Add(model);
                        }
                    }
                }
                connection.Close();
            }
            return peoples;
        }

        [HttpGet("{id}")]
        public People GetPeopleId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM People WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        People model = new People
                        {
                            id = reader.GetInt32(0),
                            firstName = reader.GetString(1),
                            middleName = reader.GetString(2),
                            lastName = reader.GetString(3),
                            emailAddress = reader.GetString(4),
                            phoneNumber = reader.GetString(5),
                            gender = reader.GetString(6),
                            dateOfBirth = reader.GetDateTime(7),
                            isDeleted = reader.GetBoolean(8),
                            createdAt = reader.GetDateTime(9),
                            updatedAt = reader.GetDateTime(10),
                        };
                        return model;
                    }
                    connection.Close();
                    return null!;
                }
            }
        }

        [HttpPost]
        public People AddPeople(People model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                var query = "INSERT INTO People ( firstName, middleName, lastName, emailAddress, phoneNumber, gender, dateOfBirth, isDeleted, createdAt, updatedAt) VALUES ( @firstName, @middleName, @lastName, @emailAddress, @phoneNumber, @gender, @dateOfBirth, @isDeleted, @createdAt, @updatedAt)";
                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        model.createdAt = DateTime.Now;
                        model.updatedAt = DateTime.Now;
                        command.Parameters.AddWithValue("@firstName", model.firstName);
                        command.Parameters.AddWithValue("@middleName", model.middleName);
                        command.Parameters.AddWithValue("@lastName", model.lastName);
                        command.Parameters.AddWithValue("@emailAddress", model.emailAddress);
                        command.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);
                        command.Parameters.AddWithValue("@gender", model.gender);
                        command.Parameters.AddWithValue("@dateOfBirth", model.dateOfBirth);
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
        public People UpdatePeople(int id, People model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE People SET firstName = @firstName, middleName = @middleName, lastName = @lastName, emailAddress = @emailAddress, phoneNumber = @phoneNumber, gender = @gender, dateOfBirth = @dateOfBirth, isDeleted = @isDeleted, createdAt  = @createdAt, updatedAt = @updatedAt WHERE id = @id";
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            model.createdAt = DateTime.Now;
                            model.updatedAt = DateTime.Now;
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@firstName", model.firstName);
                            command.Parameters.AddWithValue("@middleName", model.middleName);
                            command.Parameters.AddWithValue("@lastName", model.lastName);
                            command.Parameters.AddWithValue("@emailAddress", model.emailAddress);
                            command.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);
                            command.Parameters.AddWithValue("@gender", model.gender);
                            command.Parameters.AddWithValue("@dateOfBirth", model.dateOfBirth);
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
        public string RemovePeople(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM People WHERE id = @id", connection))
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
