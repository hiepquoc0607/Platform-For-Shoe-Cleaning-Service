using AutoMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Category;
using TP4SCS.Library.Models.Request.Service;
using TP4SCS.Library.Models.Response.Category;
using TP4SCS.Library.Models.Response.Service;

namespace TP4SCS.Library.Utils.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<Service, ServiceResponse>();
            CreateMap<ServiceRequest, Service>();

            // Category Mappings
            CreateMap<ServiceCategory, ServiceCategoryResponse>();
            CreateMap<ServiceCategoryRequest, ServiceCategory>();
        }
    }
}
