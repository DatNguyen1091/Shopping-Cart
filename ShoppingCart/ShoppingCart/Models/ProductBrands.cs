namespace ShoppingCart.Models
{
    public class ProductBrands : Common
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int brandId { get; set; }
    }
}
