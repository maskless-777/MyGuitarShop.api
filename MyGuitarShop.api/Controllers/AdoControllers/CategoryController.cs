using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repository;

namespace MyGuitarShop.api.Controllers.AdoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
        ILogger<CategoryController> logger,
        IRepository<CategoryDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var addresses = await repo.GetAllAsync();
                return Ok(addresses.Select(p => p.CategoryName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving categories");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var category = await repo.FindByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving category with category ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoryDTO dto)
        {
            try
            {
                var numCategoriesCreated = await repo.InsertAsync(dto);

                return Ok($"{numCategoriesCreated} new categories created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new categories");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, CategoryDTO updatedCategory)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Category with id {id} not found");
                var numberCategoriesUpdated = await repo.UpdateAsync(id, updatedCategory);
                return Ok($"{numberCategoriesUpdated} categories updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating category with ID {CategoryID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Category with id {id} not found");

                var numAddressesDeleted = await repo.DeleteAsync(id);
                return Ok($"{numAddressesDeleted} categories deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting categories with id {CategoryID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
