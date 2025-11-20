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
    public class CustomerRepo(ILogger<CustomerRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<CustomerDTO>
    {
        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var customers = new List<CustomerDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Customers", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var customer = new CustomerDTO
                    {
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        ShippingAddressID = reader.IsDBNull(reader.GetOrdinal("ShippingAddressID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ShippingAddressID")),
                        BillingAddressID = reader.IsDBNull(reader.GetOrdinal("BillingAddressID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                    customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Customer list");
            }
            return customers;
        }
        public async Task<CustomerDTO?> FindByIdAsync(int id)
        {
            CustomerDTO? customer = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Customers WHERE CustomerID = @CustomerID", connection);
                command.Parameters.AddWithValue("@CustomerID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    customer = new CustomerDTO()
                    {
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        ShippingAddressID = reader.IsDBNull(reader.GetOrdinal("ShippingAddressID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ShippingAddressID")),
                        BillingAddressID = reader.IsDBNull(reader.GetOrdinal("BillingAddressID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding customer {id} by ID");
            }

            return customer;
        }
        public async Task<int> InsertAsync(CustomerDTO dto)
        {
            const string query = @"
                INSERT INTO Customers (EmailAddress, Password, FirstName, LastName, ShippingAddressID, BillingAddressID)
                VALUES (@EmailAddress, @Password, @FirstName, @LastName, @ShippingAddressID, @BillingAddressID)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@FirstName", dto.FirstName);
                command.Parameters.AddWithValue("@LastName", dto.LastName);
                command.Parameters.AddWithValue("@ShippingAddressID", dto.ShippingAddressID);
                command.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new customer");
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int id, CustomerDTO dto)
        {
            const string query = @"UPDATE Customers
                                    SET EmailAddress = @EmailAddress, Password = @Password, FirstName = @FirstName, LastName = @LastName, ShippingAddressID = @ShippingAddressID, BillingAddressID = @BillingAddressID
                                    WHERE CustomerID = @CustomerID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                command.Parameters.AddWithValue("@EmailAddress", dto.EmailAddress);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@FirstName", dto.FirstName);
                command.Parameters.AddWithValue("@LastName", dto.LastName);
                command.Parameters.AddWithValue("@ShippingAddressID", dto.ShippingAddressID);
                command.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating Customer");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Customers WHERE CustomerID = @CustomerID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Customer");
                throw;
            }
        }
    }
}
