using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.Core.DTOs
{
    public class CategoriesCreateDto
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(200, ErrorMessage = "Tên danh mục không được quá 200 ký tự")]
        public string CategoryName { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
        public string? Description { get; set; }
    }
}