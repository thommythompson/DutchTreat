using System;
using DutchTreat.Data.Entities;

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
                _logger.LogInformation("Get all products was called.");

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
    }
}

