﻿namespace ShoppingCart.Models
{
    public class CartItems : Common
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public int cartId { get; set; }
        public int productId { get; set; }

        public virtual Products? Products { get; set; }
        public virtual Carts? Carts { get; set; }
    }
}
