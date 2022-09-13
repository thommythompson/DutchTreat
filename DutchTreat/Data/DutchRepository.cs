using System;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DutchTreat.Models;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _dbContext;
        private readonly ILogger _logger;

        public DutchRepository(DutchContext dbContext, ILogger<DutchRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Get all products was called.");

                return _dbContext.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation("Get products by id was called.");

                return _dbContext.Products
                .Where(p => p.Category == category)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products by id: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems = true)
        {
            try
            {
                _logger.LogInformation("Get all orders was called.");

                if (includeItems)
                {
                    return _dbContext.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
                }
                else
                {
                    return _dbContext.Orders.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders: {ex}");
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                _logger.LogInformation("Get order by id was called.");

                return _dbContext.Orders
                    .Where(o => o.Id == id)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order by id: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            try
            {
                _logger.LogInformation("Save all was called.");

                return _dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save all: {ex}");
                return false;
            }
        }

        public void AddEntity(object entity)
        {
            try
            {
                _logger.LogInformation("Add entity was called.");

                _dbContext.Add(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add entity: {ex}");
            }
        }
    }
}

