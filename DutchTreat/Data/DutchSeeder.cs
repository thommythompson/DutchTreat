using System;
using System.Text.Json;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _dbcontext;
        private readonly IHostEnvironment _env;

        public DutchSeeder(DutchContext dbcontext, IWebHostEnvironment env)
        {
            _dbcontext = dbcontext;
            _env = env;
        }

        public void Seed()
        {
            _dbcontext.Database.EnsureCreated();

            if (!_dbcontext.Products.Any())
            {
                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _dbcontext.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _dbcontext.Orders.Add(order);

                _dbcontext.SaveChanges();
            }
        }
    }
}

