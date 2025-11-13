using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repository;

namespace MyGuitarShop.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(
        ILogger<CustomersController> logger,
        IRepository<CustomerDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var customers = await repo.GetAllAsync();
                return Ok(customers.Select(p => p.EmailAddress));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving customers");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await repo.FindByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving customer with customer ID {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CustomerDTO dto)
        {
            try
            {
                var numCustomersCreated = await repo.InsertAsync(dto);

                return Ok($"{numCustomersCreated} new customers created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new customer");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerDTO updatedCustomer)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Customer with id {id} not found");
                var numberCustomersUpdated = await repo.UpdateAsync(id, updatedCustomer);
                return Ok($"{numberCustomersUpdated} customers updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating customer with ID {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Customer with id {id} not found");

                var numCustomersDeleted = await repo.DeleteAsync(id);
                return Ok($"{numCustomersDeleted} customers deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting customers with id {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
