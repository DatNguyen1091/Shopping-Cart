namespace ShoppingCart.Models
{
    public class ProductCategories : Common
    {
        public int id { get; set; }
        public int productId { get; set; }
        public int categoryId { get; set; }

        public virtual Products? Products { get; set; }
        public virtual Categories? Categories { get; set; }
    }
}
