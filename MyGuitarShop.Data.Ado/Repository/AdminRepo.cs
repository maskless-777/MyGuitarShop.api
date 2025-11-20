using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGuitarShop.Common.DTOs;

namespace MyGuitarShop.Data.Ado.Repository
{
    public class AdminRepo(
        ILogger<AdminRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<AdminDTO>
    {

        public async Task<IEnumerable<AdminDTO>> GetAllAsync()
        {
            var admins = new List<AdminDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Administrators", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var admin = new AdminDTO
                    {
                        AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    };
                    admins.Add(admin);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving admin list");
            }
            return admins;
        }
        public async Task<AdminDTO?> FindByIdAsync(int id)
        {
            AdminDTO? admin = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Administrators WHERE AdminID = @AdminID", connection);
                command.Parameters.AddWithValue("@AdminID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    admin = new AdminDTO()
                    {
                        AdminID = reader.GetInt32(reader.GetOrdinal("AdminID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding admin {id} by ID");
            }

            return admin;
        }

        public async Task<int> InsertAsync(AdminDTO dto)
        {
            const string query = @"
                INSERT INTO Administrators (EmailAddress, Password, FirstName, LastName)
                VALUES (@EmailAddress, @Password, @FirstName, @LastName)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@FirstName", dto.FirstName);
                command.Parameters.AddWithValue("@LastName", dto.LastName);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new admin");
                return 0;
            }
        }

        public async Task<int> UpdateAsync(int id, AdminDTO dto)
        {
            const string query = @"UPDATE Administrators
                                    SET EmailAddress = @EmailAddress, Password = @Password, FirstName = @FirstName, LastName = @LastName
                                    WHERE AdminID = @AdminID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AdminID", dto.AdminID);
                command.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@FirstName", dto.FirstName);
                command.Parameters.AddWithValue("@LastName", dto.LastName);
                

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating Admin");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Administrators WHERE AdminID = @AdminID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AdminID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Admin");
                throw;
            }
        }

    }
}
