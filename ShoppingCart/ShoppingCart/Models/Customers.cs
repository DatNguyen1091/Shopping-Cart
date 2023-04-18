namespace ShoppingCart.Models
{
    public class Customers : Common
    {
        public Customers()
        {
            this.Orders = new HashSet<Order>();
            this.CustomerAddresses = new HashSet<CustomerAddresses>();
        }
        public int id { get; set; }
        public int personId { get; set; }
        public bool isDeleted { get; set; }

        public virtual People? People { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<CustomerAddresses>? CustomerAddresses { get; set; }
    }
}
