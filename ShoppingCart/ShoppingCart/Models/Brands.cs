namespace ShoppingCart.Models
{
    public class Brands : Common
    {
        public Brands()
        {
            this.ProductBrands = new HashSet<ProductBrands>();
        }
        public int id { get; set; }
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? description { get; set; }
        public string? metaDescription { get; set; }
        public string? metaKeywords { get; set; }
        public string? brandStatus { get; set; }
        public bool isDelete { get; set; }

        public virtual ICollection<ProductBrands> ProductBrands { get; set; }
    }
}
