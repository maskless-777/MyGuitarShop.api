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
    public class ProductsController(
        ILogger<ProductsController> logger,
        IRepository<ProductDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var products = await repo.GetAllAsync();
                return Ok(products.Select(p => p.ProductName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await repo.FindByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving product with product ID {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductDTO dto)
        {
            try
            {
                var numProductsCreated = await repo.InsertAsync(dto);

                return Ok($"{numProductsCreated} new products created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDTO updatedProduct)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Product with id {id} not found");
                var numberProductsUpdated = await repo.UpdateAsync(id, updatedProduct);
                return Ok($"{numberProductsUpdated} products updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating product with ID {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Product with id {id} not found");

                var numProductsDeleted = await repo.DeleteAsync(id);
                return Ok($"{numProductsDeleted} products deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting products with id {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
