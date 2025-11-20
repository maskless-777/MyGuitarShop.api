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
    public class CategoryRepo(ILogger<CategoryRepo> logger,
        SqlConnectionFactory sqlConnectionFactory) : IRepository<CategoryDTO>
    {
        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = new List<CategoryDTO>();
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Categories", connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var category = new CategoryDTO
                    {
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                    categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Categories list");
            }
            return categories;
        }
        public async Task<CategoryDTO?> FindByIdAsync(int id)
        {
            CategoryDTO? category = null;

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand("SELECT * FROM Categories WHERE CategoryID = @CategoryID", connection);
                command.Parameters.AddWithValue("@CategoryID", id);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    category = new CategoryDTO()
                    {
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error finding category {id} by ID");
            }

            return category;
        }
        public async Task<int> InsertAsync(CategoryDTO dto)
        {
            const string query = @"
                INSERT INTO Categories (CategoryName)
                VALUES (@CategoryName)";

            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting new category");
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int id, CategoryDTO dto)
        {
            const string query = @"UPDATE Categories
                                    SET CategoryName = @CategoryName
                                    WHERE CategoryID = @CategoryID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", dto.CategoryID);
                command.Parameters.AddWithValue("@CategoryName", dto.CategoryName);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Updating Category");
                throw;
            }
        }
        public async Task<int> DeleteAsync(int id)
        {
            const string query = @"DELETE FROM Categories WHERE CategoryID = @CategoryID";
            try
            {
                await using var connection = await sqlConnectionFactory.OpenSqlConnectionAsync();
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", id);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error Deleting Category");
                throw;
            }
        }
    }
}
