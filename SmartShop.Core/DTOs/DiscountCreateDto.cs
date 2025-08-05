using System;
using System.ComponentModel.DataAnnotations;

namespace SmartShop.Core.DTOs
{
    public class DiscountCreateDto
    {
        [Required(ErrorMessage = "ID sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên chương trình giảm giá không được để trống")]
        [StringLength(200, ErrorMessage = "Tên chương trình không được quá 200 ký tự")]
        public string DiscountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phần trăm giảm giá là bắt buộc")]
        [Range(1, 100, ErrorMessage = "Phần trăm giảm giá phải từ 1% đến 100%")]
        public decimal DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [CustomValidation(typeof(DiscountCreateDto), "ValidateEndDate")]
        public DateTime EndDate { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string? Description { get; set; }

        public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = (DiscountCreateDto)context.ObjectInstance;
            return endDate > instance.StartDate
                ? ValidationResult.Success
                : new ValidationResult("Ngày kết thúc phải sau ngày bắt đầu");
        }
    }
}