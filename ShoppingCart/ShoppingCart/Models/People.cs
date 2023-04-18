namespace ShoppingCart.Models
{
    public class People : Common
    {
        public People()
        {
            this.Customers = new HashSet<Customers>();
        }
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public string? emailAddress { get; set; }
        public string? phoneNumber { get; set; }
        public string? gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public bool isDeleted { get; set; }
        public bool isModified { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }
}
