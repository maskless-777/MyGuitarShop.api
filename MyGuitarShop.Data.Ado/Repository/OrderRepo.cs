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
using System.Data;

namespace MyGuitarShop.Data.Ado.Repository
{
    public class OrderRepo(ILogger<OrderRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<OrderDTO>
    {
        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = new List<OrderDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Orders", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var order = new OrderDTO
                    {
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        ShipAmount = reader.GetDecimal(reader.GetOrdinal("ShipAmount")),
                        TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                        ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ShipDate")),
                        ShipAddressID = reader.GetInt32(reader.GetOrdinal("ShipAddressID")),
                        CardType = reader.GetString(reader.GetOrdinal("CardType")),
                        CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                        CardExpires = reader.GetString(reader.GetOrdinal("CardExpires")),
                        BillingAddressID = reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                    };
                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Order list");
            }
            return orders;
        }
        public async Task<OrderDTO?> FindByIdAsync(int id)
        {
            OrderDTO? order = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Orders WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    order = new OrderDTO()
                    {
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        ShipAmount = reader.GetDecimal(reader.GetOrdinal("ShipAmount")),
                        TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                        ShipDate = reader.IsDBNull(reader.GetOrdinal("ShipDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ShipDate")),
                        ShipAddressID = reader.GetInt32(reader.GetOrdinal("ShipAddressID")),
                        CardType = reader.GetString(reader.GetOrdinal("CardType")),
                        CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                        CardExpires = reader.GetString(reader.GetOrdinal("CardExpires")),
                        BillingAddressID = reader.GetInt32("BillingAddressID")
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding order {id} by ID");
            }

            return order;
        }
        public async Task<int> InsertAsync(OrderDTO dto)
        {
            const string query = @"
                INSERT INTO Orders (CustomerID, OrderDate, ShipAmount, TaxAmount, ShipDate, ShipAddressID, CardType, CardNumber, CardExpires, BillingAddressID)
                VALUES (@CustomerID, @OrderDate, @ShipAmount, @TaxAmount, @ShipDate, @ShipAddressID, @CardType, @CardNumber, @CardExpires, @BillingAddressID)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                command.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
                command.Parameters.AddWithValue("@ShipAmount", dto.ShipAmount);
                command.Parameters.AddWithValue("@TaxAmount", dto.TaxAmount);
                command.Parameters.AddWithValue("@ShipDate", dto.ShipDate);
                command.Parameters.AddWithValue("@ShipAddressID", dto.ShipAddressID);
                command.Parameters.AddWithValue("@CardType", dto.CardType);
                command.Parameters.AddWithValue("@CardNumber", dto.CardNumber);
                command.Parameters.AddWithValue("@CardExpires", dto.CardExpires);
                command.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new order");
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int id, OrderDTO dto)
        {
            const string query = @"UPDATE Orders
                                    SET CustomerID = @CustomerID, OrderDate = @OrderDate, ShipAmount = @ShipAmount, TaxAmount = @TaxAmount, ShipDate = @ShipDate, ShipAddressID = @ShipAddressID, CardType = @CardType, CardNumber = @CardNumber, CardExpires = @CardExpires, BillingAddressID = @BillingAddressID
                                    WHERE OrderID = @OrderID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", dto.OrderID);
                command.Parameters.AddWithValue("@CustomerID", dto.CustomerID);
                command.Parameters.AddWithValue("@OrderDate", dto.OrderDate);
                command.Parameters.AddWithValue("@ShipAmount", dto.ShipAmount);
                command.Parameters.AddWithValue("@TaxAmount", dto.TaxAmount);
                command.Parameters.AddWithValue("@ShipDate", dto.ShipDate);
                command.Parameters.AddWithValue("@ShipAddressID", dto.ShipAddressID);
                command.Parameters.AddWithValue("@CardType", dto.CardType);
                command.Parameters.AddWithValue("@CardNumber", dto.CardNumber);
                command.Parameters.AddWithValue("@CardExpires", dto.CardExpires);
                command.Parameters.AddWithValue("@BillingAddressID", dto.BillingAddressID);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating order");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Orders WHERE OrderID = @OrderID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Order");
                throw;
            }
        }
    }
}
