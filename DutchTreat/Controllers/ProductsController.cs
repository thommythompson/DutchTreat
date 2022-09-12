using System;
using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data.Entities;
using System.Text.Json;

namespace DutchTreat.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository repo, ILogger<ProductsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                return Ok(_repo.GetAllProducts());
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }
    }
}

