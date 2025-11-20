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
    public class OrderItemsController(
        ILogger<OrderItemsController> logger,
        IRepository<OrderItemDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var orderitems = await repo.GetAllAsync();
                return Ok(orderitems.Select(p => p.ItemID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orderitems");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var orderitem = await repo.FindByIdAsync(id);
                if (orderitem == null)
                {
                    return NotFound();
                }
                return Ok(orderitem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orderitem with item ID {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync(OrderItemDTO dto)
        {
            try
            {
                var numItemsCreated = await repo.InsertAsync(dto);

                return Ok($"{numItemsCreated} new orderitems created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new orderitem");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItemAsync(int id, OrderItemDTO updatedItem)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"OrderItem with id {id} not found");
                var numberItemsUpdated = await repo.UpdateAsync(id, updatedItem);
                return Ok($"{numberItemsUpdated} orderitems updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating orderitem with ID {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"OrderItem with id {id} not found");

                var numItemsDeleted = await repo.DeleteAsync(id);
                return Ok($"{numItemsDeleted} orderitems deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting orderitems with id {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
