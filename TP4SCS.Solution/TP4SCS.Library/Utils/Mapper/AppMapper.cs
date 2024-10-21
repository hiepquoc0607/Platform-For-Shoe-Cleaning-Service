using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Models.Request.CartItem;
using TP4SCS.Library.Models.Request.Category;
using TP4SCS.Library.Models.Request.Promotion;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.Category;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<Service, ServiceResponse>();
            CreateMap<Service, ServiceCreateResponse>();
            CreateMap<ServiceRequest, Service>();
            CreateMap<ServiceCreateRequest, Service>().ForMember(dest => dest.BranchId, opt => opt.Ignore());

            // Category Mappings
            CreateMap<ServiceCategory, ServiceCategoryResponse>();
            CreateMap<ServiceCategoryRequest, ServiceCategory>();
            //Cart Mappings
            CreateMap<CartUpdateRequest, Cart>();
            CreateMap<Cart, CartResponse>();
            //Cart Item Mappings
            CreateMap<CartItemCreateRequest, CartItem>();
            CreateMap<CartItem, CartItemResponse>();
            //Promoation Mappings
            CreateMap<Promotion, PromotionResponse>();
            CreateMap<PromotionCreateRequest, Promotion>();
            CreateMap<PromotionUpdateRequest, Promotion>();
        }
    }
}
