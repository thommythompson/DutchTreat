using System;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using AutoMapper;

namespace DutchTreat.Controllers
{
    [Route("Api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repo;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repo, ILogger<OrdersController> logger, IMapper mapper) 
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var result = _repo.GetAllOrders(includeItems);

                return Ok(_mapper.Map<IEnumerable<OrderModel>>(result));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unable to get all orders: {ex}");
                return BadRequest("Unable to get all orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repo.GetOrderById(id);

                if (order != null) return Ok(_mapper.Map<OrderModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get order by id: {ex}");
                return BadRequest("Unable to get order by id");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderModel, Order>(model);

                    if(newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    _repo.AddEntity(newOrder);

                    if (_repo.SaveAll())
                    {
                        var order = new OrderModel()
                        {
                            OrderId = newOrder.Id,
                            OrderDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber,
                        };

                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save order: {ex}");
            }
            return BadRequest("Unable to save order");
        }
    }
}