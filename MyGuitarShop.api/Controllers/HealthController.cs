using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.EFCore.Context;

namespace MyGuitarShop.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(
         ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory,
        MyGuitarShopContext dbContext)
        : ControllerBase
    {
        [HttpGet]

        public IActionResult Get()
        {
            try
            {
                return Ok("Healthy");
            }
            catch (Exception ex)
            {
                logger.LogWarning("Health check failed unreasonably.");
                return StatusCode(503, "Unhealthy");
            }
        }

        [HttpGet("db/ado")]
        public IActionResult GetDbHealth()
        {
            try
            {
                using var connection = sqlConnectionFactory.OpenSqlConnection();
                return Ok(new { Message = "Connection Successful!", connection.Database });
            }
            catch (Exception)
            {
                logger.LogCritical("Database health chceck failed.");
                return StatusCode(503, "Database Unhealthy");
            }
        }

        [HttpGet("db/efcore")]
        public async Task<IActionResult> GetDbContextHealthAsync()
        {
            try
            {
                if (!await dbContext.Database.CanConnectAsync())
                    throw new Exception("Cannot connect to database via Ef Core DbContext.");
                return Ok(new { Message = "Connection successful!", dbContext.Database });
            }
            catch (Exception)
            {
                logger.LogCritical("Database health check failed.");
                return StatusCode(503, "Database Unhealthy");
            }
        }

    }
}
