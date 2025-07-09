using AutoMapper;
using SmartShop.Core.DTOs;
using SmartShop.Web.Models;

namespace SmartShop.Web.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DTO to ViewModel
            CreateMap<ProductDto, ProductViewModel>();
           

            // ViewModel to DTO
            CreateMap<ProductViewModel, ProductDto>();
           
        }
    }
}
