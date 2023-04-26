using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Data.SqlClient;
using System.Transactions;
using X.PagedList;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {

        [HttpGet]
        public List<People> GetPeople(int? page)
        {
            List<People> peoples = new List<People>();
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
            var people = peoples.ToPagedList(pageIndex, pageSize);
            return people.ToList();
        }

        [HttpGet("{id}")]
        public People GetPeopleId(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "INSERT INTO People ( firstName, middleName, lastName, emailAddress, phoneNumber, gender, dateOfBirth, isDeleted, createdAt) VALUES ( @firstName, @middleName, @lastName, @emailAddress, @phoneNumber, @gender, @dateOfBirth, @isDeleted, @createdAt)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", model.firstName);
                    command.Parameters.AddWithValue("@middleName", model.middleName);
                    command.Parameters.AddWithValue("@lastName", model.lastName);
                    command.Parameters.AddWithValue("@emailAddress", model.emailAddress);
                    command.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);
                    command.Parameters.AddWithValue("@gender", model.gender);
                    command.Parameters.AddWithValue("@dateOfBirth", model.dateOfBirth);
                    command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
                    command.Parameters.AddWithValue("@createdAt", model.createdAt);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                return model;
            }
        }

        [HttpPut("{id}")]
        public People UpdatePeople(int id, People model)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                connection.Open();
                var query = "UPDATE People SET firstName = @firstName, middleName = @middleName, lastName = @lastName, emailAddress = @emailAddress, phoneNumber = @phoneNumber, gender = @gender, dateOfBirth = @dateOfBirth, isDeleted = @isDeleted, updatedAt = @updatedAt WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@firstName", model.firstName);
                    command.Parameters.AddWithValue("@middleName", model.middleName);
                    command.Parameters.AddWithValue("@lastName", model.lastName);
                    command.Parameters.AddWithValue("@emailAddress", model.emailAddress);
                    command.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);
                    command.Parameters.AddWithValue("@gender", model.gender);
                    command.Parameters.AddWithValue("@dateOfBirth", model.dateOfBirth);
                    command.Parameters.AddWithValue("@isDeleted", model.isDeleted);
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
        public string RemovePeople(int id)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
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
