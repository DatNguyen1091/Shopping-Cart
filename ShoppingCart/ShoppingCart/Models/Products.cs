namespace ShoppingCart.Models
{
    public class Products : Common
    {
        public Products()
        {
            this.ProductBrands = new HashSet<ProductBrands>();
            this.OrderItems = new HashSet<OrderItems>();
            this.ProductCategories = new HashSet<ProductCategories>();
            this.CartItems = new HashSet<CartItems>();
        }
        public int id { get; set; }
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? description { get; set; }
        public string? metaDescription { get; set; }
        public string? sku { get; set; }
        public string? model { get; set; }
        public decimal price { get; set; }
        public decimal oldPrice { get; set; }
        public string? imageUrl { get; set; }
        public bool isBestseller { get; set; }
        public bool isFeatured { get; }
        public int quantity { get; set; }
        public string? productStatus { get; set; }
        public bool isDeleted { get; set; }

        public virtual ICollection<ProductBrands> ProductBrands { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ProductCategories> ProductCategories { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
