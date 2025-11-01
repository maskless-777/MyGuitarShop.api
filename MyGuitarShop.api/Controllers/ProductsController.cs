using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repository;

namespace MyGuitarShop.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        ProductRepo repo) : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await repo.GetAllProductsAsync();
                return Ok(products.Select(p=>p.ProductName));
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Error retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
