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
    public class AdminController(
        ILogger<AdminController> logger,
        IRepository<AdminDTO> repo)
        : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var addresses = await repo.GetAllAsync();
                return Ok(addresses.Select(p => p.EmailAddress));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving admins");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var admin = await repo.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving admin with admin ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminAsync(AdminDTO dto)
        {
            try
            {
                var numAdminsCreated = await repo.InsertAsync(dto);

                return Ok($"{numAdminsCreated} new admins created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new admin");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdminAsync(int id, AdminDTO updatedAdmin)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Admin with id {id} not found");
                var numberAdminsUpdated = await repo.UpdateAsync(id, updatedAdmin);
                return Ok($"{numberAdminsUpdated} admins updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating admin with ID {AdminID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Admin with id {id} not found");

                var numAdminsDeleted = await repo.DeleteAsync(id);
                return Ok($"{numAdminsDeleted} admins deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting admins with id {AdminID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }
    }
}
