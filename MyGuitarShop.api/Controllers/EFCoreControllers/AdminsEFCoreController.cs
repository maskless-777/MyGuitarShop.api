using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.api.Abstract;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.EFCore.Entities;
using MyGuitarShop.Data.EFCore.Repositories;

namespace MyGuitarShop.api.Controllers.EFCoreControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsEFCoreController(
        AdminRepository repository,
        ILogger<AdminsEFCoreController> logger)
        : BaseController<AdminDTO, Administrator>(repository, logger) { }
}
