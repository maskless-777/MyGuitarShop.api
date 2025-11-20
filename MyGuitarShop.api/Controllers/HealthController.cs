using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyGuitarShop.Data.Ado.Factories;
using MongoDB.Driver;

namespace MyGuitarShop.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(
         ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory,
        IMongoClient mongoClient)
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


        [HttpGet("db/mongo")]
        public async Task<IActionResult> GetMongoDbHealthAsync()
        {
            try
            {
                var response = await mongoClient.ListDatabaseNamesAsync();
                var databaseNames = await response.ToListAsync() ?? [];
                if (databaseNames.Count == 0)
                    throw new Exception("Cannot connect to Mongo Database.");

                return Ok(new {Message = "Connection Successful!", databaseNames});
            }
            catch (Exception)
            {
                logger.LogCritical("Mongo Database health chceck failed.");
                return StatusCode(503, "Database Unhealthy");
            }
        }
    }
}
