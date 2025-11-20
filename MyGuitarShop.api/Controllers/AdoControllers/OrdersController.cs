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
    public class OrdersController(
        ILogger<OrdersController> logger,
        IRepository<OrderDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var orders = await repo.GetAllAsync();
                return Ok(orders.Select(p => p.OrderID));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orders");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var order = await repo.FindByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving order with order ID {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderDTO dto)
        {
            try
            {
                var numOrdersCreated = await repo.InsertAsync(dto);

                return Ok($"{numOrdersCreated} new orders created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new order");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, OrderDTO updatedOrder)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order with id {id} not found");
                var numberOrdersUpdated = await repo.UpdateAsync(id, updatedOrder);
                return Ok($"{numberOrdersUpdated} orders updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order with ID {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Order with id {id} not found");

                var numOrdersDeleted = await repo.DeleteAsync(id);
                return Ok($"{numOrdersDeleted} orders deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting orders with id {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
