namespace SmartShop.Core.Entities
{
    public class Category : IEntity, IAuditable
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}