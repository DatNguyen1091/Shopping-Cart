namespace ShoppingCart.Models
{
    public class Carts : Common
    {
        public int id { get; set; }
        public string? uniqueCartId { get; set; }
        public string? cartStatus { get; set; }
    }
}
