namespace XmlLoader.Models
{
    public class SalesOfOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }

        public double Value { get; set; }
    }
}
