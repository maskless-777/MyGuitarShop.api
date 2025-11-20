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
    public class OrderItemRepo(ILogger<OrderItemRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<OrderItemDTO>
    {
        public async Task<IEnumerable<OrderItemDTO>> GetAllAsync()
        {
            var orderitems = new List<OrderItemDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM OrderItems", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var orderitem = new OrderItemDTO
                    {
                        ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                        OrderID = reader.IsDBNull(reader.GetOrdinal("OrderID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ProductID = reader.IsDBNull(reader.GetOrdinal("ProductID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ItemPrice = reader.GetDecimal(reader.GetOrdinal("ItemPrice")),
                        DiscountAmount = reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    };
                    orderitems.Add(orderitem);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving OrderItem list");
            }
            return orderitems;
        }
        public async Task<OrderItemDTO?> FindByIdAsync(int id)
        {
            OrderItemDTO? orderitem = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM OrderItems WHERE ItemID = @ItemID", connection);
                command.Parameters.AddWithValue("@ItemID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    orderitem = new OrderItemDTO()
                    {
                        ItemID = reader.GetInt32(reader.GetOrdinal("ItemID")),
                        OrderID = reader.IsDBNull(reader.GetOrdinal("OrderID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ProductID = reader.IsDBNull(reader.GetOrdinal("ProductID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("OrderID")),
                        ItemPrice = reader.GetDecimal(reader.GetOrdinal("ItemPrice")),
                        DiscountAmount = reader.GetDecimal(reader.GetOrdinal("DiscountAmount")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding orderitem {id} by ID");
            }

            return orderitem;
        }
        public async Task<int> InsertAsync(OrderItemDTO dto)
        {
            const string query = @"
                INSERT INTO OrderItems (OrderID, ProductID, ItemPrice, DiscountAmount, Quantity)
                VALUES (@OrderID, @ProductID, @ItemPrice, @DiscountAmount, @Quantity)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", dto.OrderID);
                command.Parameters.AddWithValue("@ProductID", dto.ProductID);
                command.Parameters.AddWithValue("@ItemPrice", dto.ItemPrice);
                command.Parameters.AddWithValue("@DiscountAmount", dto.DiscountAmount);
                command.Parameters.AddWithValue("@Quantity", dto.Quantity);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new orderitem");
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int id, OrderItemDTO dto)
        {
            const string query = @"UPDATE OrderItems
                                    SET OrderID = @OrderID, ProductID = @ProductID, ItemPrice = @ItemPrice, DiscountAmount = @DiscountAmount, Quantity = @Quantity
                                    WHERE ItemID = @ItemID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemID", dto.ItemID);
                command.Parameters.AddWithValue("@OrderID", dto.OrderID);
                command.Parameters.AddWithValue("@ProductID", dto.ProductID);
                command.Parameters.AddWithValue("@ItemPrice", dto.ItemPrice);
                command.Parameters.AddWithValue("@DiscountAmount", dto.DiscountAmount);
                command.Parameters.AddWithValue("@Quantity", dto.Quantity);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating OrderItem");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM OrderItems WHERE ItemID = @ItemID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting OrderItem");
                throw;
            }
        }
    }
}
