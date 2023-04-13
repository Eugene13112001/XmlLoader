namespace XmlLoader.Models
{
  
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public ICollection<SalesOfOrder> SalesOfOrders { get; set; }
    }
}
