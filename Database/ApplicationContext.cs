using Microsoft.EntityFrameworkCore;
using XmlLoader.Models;

namespace XmlLoader.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesOfOrder> SalesOfOrders { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public async Task<Product?> GetProduct(Product prod)
        {
            return await this.Products.FirstOrDefaultAsync(c => c.Name == prod.Name);
        }
        public async Task<User?> GetUser(User user)
        {
            return await this.Users.FirstOrDefaultAsync(c => (c.Name == user.Name) && (c.Email == user.Email));
        }
        public async Task<Product> AddProduct(Product product)
        {

            await this.Products.AddAsync(product);
            await this.SaveChangesAsync();
            return product;

        }
        public async Task<User> AddUser(User user)
        {

            await this.Users.AddAsync(user);
            await this.SaveChangesAsync();
            return user;

        }

        public async Task<Order> AddOrder(Order order)
        {
            await this.Orders.AddAsync(order);
            await this.SaveChangesAsync();
            return order;

        }
        public async Task<SalesOfOrder> AddSale(SalesOfOrder sale)
        {

            await this.SalesOfOrders.AddAsync(sale);
            await this.SaveChangesAsync();
            return sale;

        }
        public async Task<Order?> GetOrder(int numb)
        {
            return await this.Orders.FirstOrDefaultAsync(c => (c.Number == numb));
        }

    }
}
