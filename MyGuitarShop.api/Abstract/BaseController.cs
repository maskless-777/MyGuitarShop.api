using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.api.Mappers;

namespace MyGuitarShop.api.Abstract
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TEntity>(
        IRepository<TEntity> repo,
        ILogger<BaseController<TDto, TEntity>> logger
        ) : ControllerBase where TEntity : class, new()
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var entities = await repo.GetAllAsync();
                return entities.Any() ? Ok(entities) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching entities");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var entity = await repo.FindByIdAsync(id);

                if (entity == null)
                    return NotFound($"Entity with id {id} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Entity");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TDto dto)
        {
            try
            {
                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto)
                    ?? throw new InvalidOperationException("Unable to map Dto to Entity");

                var entitiesCreated = await repo.InsertAsync(entity);

                return Ok($"{entitiesCreated} entities created.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new entities");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, TDto dto)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Entity with id {id} not found");

                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto)
                    ?? throw new InvalidOperationException("Unable to map Dto to Entity");

                var numberUpdated = await repo.UpdateAsync(id, entity);

                return Ok($"{numberUpdated} entities updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
