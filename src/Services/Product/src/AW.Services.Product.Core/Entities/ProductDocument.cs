namespace AW.Services.Product.Core.Entities
{
    public class ProductDocument
    {
        public int Id { get; set; }
        public string? DocumentNode { get; private set; }
        public Document? Document { get; private set; }
        public DateTime ModifiedDate { get; private set; }
    }
}