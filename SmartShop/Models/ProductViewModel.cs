using SmartShop.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace SmartShop.Web.Models
{
    public class ProductViewModel
    {

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }

      
    }
}
