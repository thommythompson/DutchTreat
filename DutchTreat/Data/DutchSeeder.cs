using System;
using System.Collections.Concurrent;
using System.Text.Json;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _dbcontext;
        private readonly IHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;
        private readonly ILogger<DutchSeeder> _logger;

        public DutchSeeder(DutchContext dbcontext, IWebHostEnvironment env, UserManager<StoreUser> userManager, ILogger<DutchSeeder> logger)
        {
            _dbcontext = dbcontext;
            _env = env;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Start seeding");
            _dbcontext.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("tho@thomashofmn.nl");
            if(user == null)
            {
                _logger.LogInformation("Seeding default user");

                user = new StoreUser()
                {
                    FirstName = "Thomas",
                    LastName = "Hofman",
                    Email = "tho@thomashofmn.nl",
                    UserName = "tho@thomashofmn.nl"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not creat new user in seeder");
                }
            }

            if (!_dbcontext.Products.Any())
            {
                _logger.LogInformation("Seeding default products");

                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _dbcontext.AddRange(products);

                var order = _dbcontext.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if(order == null)
                {
                    order = new Order()
                    {
                        User = user,
                        OrderDate = DateTime.Now,
                        OrderNumber = "12345",
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
                }
                
                _dbcontext.SaveChanges();
            }
        }
    }
}

