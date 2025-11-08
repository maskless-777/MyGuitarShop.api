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
    public class ProductRepo(
        ILogger<ProductRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<ProductDTO>
    {

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = new List<ProductDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Products", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var product = new ProductDTO
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product list");
            }
            return products;
        }
        public async Task<ProductDTO?> FindByIdAsync(int id)
        {
            ProductDTO? product = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", connection);
                command.Parameters.AddWithValue("@ProductID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    product = new ProductDTO()
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.IsDBNull(reader.GetOrdinal("DateAdded")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding product {id} by ID");
            }



            return product;
        }

        public async Task<int> InsertAsync(ProductDTO dto)
        {
            const string query = @"
                INSERT INTO Products (CategoryID, ProductCode, ProductName, Description, ListPrice, DiscountPercent, DateAdded)
                VALUES (@CategoryID, @ProductCode, @ProductName, @Description, @ListPrice, @DiscountPercent, @DateAdded)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", dto.CategoryID);
                command.Parameters.AddWithValue("@ProductCode", dto.ProductCode);
                command.Parameters.AddWithValue("@ProductName", dto.ProductName);
                command.Parameters.AddWithValue("@Description", dto.Description);
                command.Parameters.AddWithValue("@ListPrice", dto.ListPrice);
                command.Parameters.AddWithValue("@DiscountPercent", dto.DiscountPercent);
                command.Parameters.AddWithValue("@DateAdded", dto.DateAdded);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new product");
                return 0;
            }
        }

        public async Task<int> UpdateAsync(int id, ProductDTO dto)
        {
            const string query = @"UPDATE Products
                                    SET CategoryID = @CategoryID, ProductCode = @ProductCode, ProductName = @ProductName, Description = @Description, ListPrice = @ListPrice, DiscountPercent = @DiscountPercent
                                    WHERE ProductID = @ProductID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", dto.CategoryID);
                command.Parameters.AddWithValue("@ProductCode", dto.ProductCode);
                command.Parameters.AddWithValue("@ProductID", dto.ProductID);
                command.Parameters.AddWithValue("@ProductName", dto.ProductName);
                command.Parameters.AddWithValue("@Description", dto.Description);
                command.Parameters.AddWithValue("@ListPrice", dto.ListPrice);
                command.Parameters.AddWithValue("@DiscountPercent", dto.DiscountPercent);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating Product");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Products WHERE ProductID = @ProductID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Product");
                throw;
            }
        }

    }
}
