using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XmlLoader.Database;
using XmlLoader.Models;
using XmlLoader.XMLModels;

namespace XmlLoader.Services.Impl
{
    public class DocumentLoader : IDocumentLoaderService
    {

        private readonly ApplicationContext _dbContext;
        private readonly ILogger<DocumentLoader> _logger;

        public DocumentLoader(
            ILogger<DocumentLoader> logger,
            ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UploadAsync(string fileName)
        {
            XmlSerializer xmlSerializer = new(typeof(XmlListOrders));
            using Stream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var data = (XmlListOrders?)xmlSerializer.Deserialize(fStream);

            if (data?.orders != null)
            {
                foreach (var order in data.orders!)
                {
                    try
                    {
                        var orderDb = _dbContext.Orders.FirstOrDefault(item => item.Number == order.No);
                        if (orderDb != null)
                            throw new Exception($"Заявка с номером {order.No} уже зарегистрирована в системе.");

                        // get/add user
                        var user = _dbContext.Users.FirstOrDefault(user => user.Email == order.User.Email);
                        if (user == null)
                        {
                            user = new User
                            {
                                Name = order.User.Name,
                                Email = order.User.Email
                            };
                            _dbContext.Users.Add(user);

                            await _dbContext.SaveChangesAsync();
                        }

                        var products = new Dictionary<Product, XmlProduct>();

                        //get/add all products
                        foreach (var orderProduct in order.Products)
                        {
                            var product = _dbContext.Products.FirstOrDefault(product => product.Name == orderProduct.Name);
                            if (product == null)
                            {
                                product = new Product
                                {
                                    Name = orderProduct.Name,
                                    Price = orderProduct.Price
                                };
                                _dbContext.Products.Add(product);
                                await _dbContext.SaveChangesAsync();
                            }
                            products.Add(product, orderProduct);
                        }

                        // create order
                        var newOrder = new Order
                        {
                            Date = order.Date,
                            Number = order.No,
                            Sum = order.Sum,
                            UserId = user.Id,
                            SalesOfOrder = products.Select(item => new SalesOfOrder
                            {
                                ProductId = item.Key.Id,
                                Count = item.Value.Count

                            }).ToList()
                        };
                        _dbContext.Orders.Add(newOrder);
                        await _dbContext.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при попытке обработать входящий документ.");
                    }

                }
            }
        }
    }
}
