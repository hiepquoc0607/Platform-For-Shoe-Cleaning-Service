using AutoMapper;
using Mapster;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.AssetUrl;
using TP4SCS.Library.Models.Request.CartItem;
using TP4SCS.Library.Models.Request.Category;
using TP4SCS.Library.Models.Request.Feedback;
using TP4SCS.Library.Models.Request.Material;
using TP4SCS.Library.Models.Request.Promotion;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.AssetUrl;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.BranchService;
using TP4SCS.Library.Models.Response.BusinessProfile;
using TP4SCS.Library.Models.Response.Cart;
using TP4SCS.Library.Models.Response.CartItem;
using TP4SCS.Library.Models.Response.Category;
using TP4SCS.Library.Models.Response.Material;
using TP4SCS.Library.Models.Response.Order;
using TP4SCS.Library.Models.Response.OrderDetail;
using TP4SCS.Library.Models.Response.Promotion;
using TP4SCS.Library.Models.Response.Service;
using TP4SCS.Library.Utils.Utils;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AppMapper : Profile, IRegister
    {
        public AppMapper()
        {
            CreateMap<Service, ServiceResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            CreateMap<Service, ServiceCreateResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            CreateMap<ServiceRequest, Service>();
            CreateMap<ServiceCreateRequest, Service>();

            // Category Mappings
            CreateMap<ServiceCategory, ServiceCategoryResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            CreateMap<ServiceCategoryRequest, ServiceCategory>();
            //Cart Mappings
            CreateMap<Cart, CartResponse>();
            //Cart Item Mappings
            CreateMap<CartItemCreateRequest, CartItem>();
            CreateMap<CartItem, CartItemResponse>();
            //Promotion Mappings
            CreateMap<Promotion, PromotionResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            CreateMap<PromotionCreateRequest, Promotion>();
            CreateMap<PromotionUpdateRequest, Promotion>();
            //Material Mappings
            CreateMap<MaterialCreateRequest, Material>();
            CreateMap<MaterialUpdateRequest, Material>();
            CreateMap<Material, MaterialResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            //AssetUrl Mapping
            CreateMap<AssetUrl, AssetUrlResponse>();
            CreateMap<AssetUrlRequest, AssetUrl>();
            //BranchService Mapping
            CreateMap<BranchService, BranchServiceResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateGeneralStatus(src.Status)));
            //BusinessBranch Mapping
            CreateMap<BusinessBranch, BranchResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateBranchStatus(src.Status)));
            //Feedback Mapping
            CreateMap<FeedbackRequest, Feedback>();
            //Order Mapping
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateOrderStatus(src.Status)))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email));
            //OrderDetail Mapping
            CreateMap<OrderDetail, OrderDetailResponse>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateOrderDetailStatus(src.Status)));
            CreateMap<OrderDetail, OrderDetailResponseV2>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Util.TranslateOrderDetailStatus(src.Status)));
        }

        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<OrderDetail, OrderDetailResponse>
                .NewConfig()
                .Map(dest => dest.Status, src => Util.TranslateOrderDetailStatus(src.Status));
            TypeAdapterConfig<OrderDetail, OrderDetailResponseV2>
                .NewConfig()
                .Map(dest => dest.Status, src => Util.TranslateOrderDetailStatus(src.Status));
            TypeAdapterConfig<Order, OrderResponse>
                .NewConfig()
                .Map(dest => dest.Status, src => Util.TranslateOrderStatus(src.Status))
                .Map(dest => dest.OrderDetails, src => src.OrderDetails.Adapt<List<OrderDetailResponseV2>>())
                .Map(dest => dest.Phone, src => src.Account.Phone)
                .Map(dest => dest.FullName, src => src.Account.FullName)
                .Map(dest => dest.Email, src => src.Account.Email);

        }
    }
}
