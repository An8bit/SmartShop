using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShop.Core.Entities
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public bool IsCurrentlyActive()
        {
            var now = DateTime.Now;
            return IsActive && now >= StartDate && now <= EndDate;
        }
    }
}