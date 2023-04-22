namespace ShoppingCart.Models
{
    public class Customers : Common
    {
        public int id { get; set; }
        public int personId { get; set; }
        public bool isDeleted { get; set; }
    }
}
