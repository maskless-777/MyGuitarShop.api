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
    public class AddressRepo(ILogger<AddressRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<AddressDTO>
    {
        public async Task<IEnumerable<AddressDTO>> GetAllAsync()
        {
            var addresses = new List<AddressDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Addresses", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var address = new AddressDTO
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        Line1 = reader.GetString(reader.GetOrdinal("Line1")),
                        Line2 = reader.IsDBNull(reader.GetOrdinal("Line2")) ? (string?)null : reader.GetString(reader.GetOrdinal("Line2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        Disabled = reader.GetInt32(reader.GetOrdinal("Disabled"))
                    };
                    addresses.Add(address);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Address list");
            }
            return addresses;
        }
        public async Task<AddressDTO?> FindByIdAsync(int id)
        {
            AddressDTO? address = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Addresses WHERE AddressID = @AddressID", connection);
                command.Parameters.AddWithValue("@AddressID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    address = new AddressDTO()
                    {
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        Line1 = reader.GetString(reader.GetOrdinal("Line1")),
                        Line2 = reader.IsDBNull(reader.GetOrdinal("Line2")) ? (string?)null : reader.GetString(reader.GetOrdinal("Line2")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        Disabled = reader.GetInt32(reader.GetOrdinal("Disabled"))
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding address {id} by ID");
            }

            return address;
        }
        public async Task<int> InsertAsync(AddressDTO dto)
        {
            const string query = @"
                INSERT INTO Addresses (CustomerID, Line1, Line2, City, State, ZipCode, Phone, Disabled)
                VALUES (@CustomerID, @Line1, @Line2, @City, @State, @ZipCode, @Phone, @Disabled)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                command.Parameters.AddWithValue("@Line1", dto.Line1);
                command.Parameters.AddWithValue("@Line2", dto.Line2);
                command.Parameters.AddWithValue("@City", dto.City);
                command.Parameters.AddWithValue("@State", dto.State);
                command.Parameters.AddWithValue("@ZipCode", dto.ZipCode);
                command.Parameters.AddWithValue("@Phone", dto.Phone);
                command.Parameters.AddWithValue("@Disabled", dto.Disabled);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new product");
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int id, AddressDTO dto)
        {
            const string query = @"UPDATE Addresses
                                    SET CustomerID = @CustomerID, Line1 = @Line1, Line2 = @Line2, City = @City, State = @State, ZipCode = @ZipCode, Disabled = @Disabled
                                    WHERE AddressID = @AddressID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AddressID", dto.AddressID);
                command.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                command.Parameters.AddWithValue("@Line1", dto.Line1);
                command.Parameters.AddWithValue("@Line2", dto.Line2);
                command.Parameters.AddWithValue("@City", dto.City);
                command.Parameters.AddWithValue("@State", dto.State);
                command.Parameters.AddWithValue("@ZipCode", dto.ZipCode);
                command.Parameters.AddWithValue("@Phone", dto.Phone);
                command.Parameters.AddWithValue("@Disabled", dto.Disabled);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating Address");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Addresses WHERE AddressID = @AddressID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AddressID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Address");
                throw;
            }
        }
    }
}
