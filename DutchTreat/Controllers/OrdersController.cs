using System;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Data.Entities;

namespace DutchTreat.Controllers
{
    [Route("Api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IDutchRepository repo, ILogger<OrdersController> logger) 
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            try
            {
                return Ok(_repo.GetAllOrders());
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unable to get all orders: {ex}");
                return BadRequest("Unable to get all orders");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var order = _repo.GetOrderById(id);

                if (order != null) return Ok(order);
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get order by id: {ex}");
                return BadRequest("Unable to get order by id");
            }
        }
    }
}

