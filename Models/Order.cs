namespace XmlLoader.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public double Sum { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }
       
        public List<SalesOfOrder> SalesOfOrder { get; set; }
    }
}
