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
    public class AddressesController(
        ILogger<AddressesController> logger,
        IRepository<AddressDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var addresses = await repo.GetAllAsync();
                return Ok(addresses.Select(p => p.Line1));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving addresses");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var address = await repo.FindByIdAsync(id);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving address with address ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync(AddressDTO dto)
        {
            try
            {
                var numAddressesCreated = await repo.InsertAsync(dto);

                return Ok($"{numAddressesCreated} new addresses created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new address");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id, AddressDTO updatedAddress)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Address with id {id} not found");
                var numberAddressesUpdated = await repo.UpdateAsync(id, updatedAddress);
                return Ok($"{numberAddressesUpdated} addresses updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating address with ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Address with id {id} not found");

                var numAddressesDeleted = await repo.DeleteAsync(id);
                return Ok($"{numAddressesDeleted} addresses deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting addresses with id {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
