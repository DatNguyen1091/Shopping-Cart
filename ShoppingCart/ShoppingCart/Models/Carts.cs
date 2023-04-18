namespace ShoppingCart.Models
{
    public class Carts : Common
    {
        public Carts()
        {
            this.CartItems = new HashSet<CartItems>();
        }
        public int id { get; set; }
        public string? uniqueCartId { get; set; }
        public string? cartStatus { get; set; }

        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
