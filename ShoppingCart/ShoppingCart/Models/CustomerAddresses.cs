namespace ShoppingCart.Models
{
    public class CustomerAddresses : Common
    {
        public int id { get; set; }
        public int customerId { get; set; }
        public int addressId { get; set; }

        public virtual Addresses? Addresses { get; set; }
        public virtual Customers? Customers { get; set; }
    }
}
