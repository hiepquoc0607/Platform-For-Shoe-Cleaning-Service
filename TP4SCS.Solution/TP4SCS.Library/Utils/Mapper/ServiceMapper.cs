using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request;
using TP4SCS.Library.Models.Response;

namespace TP4SCS.Library.Utils.Mapper
{
    public class ServiceMapper : Profile
    {
        public ServiceMapper()
        {
            CreateMap<Service, ServiceResponse>();
            CreateMap<ServiceRequest, Service>();
        }
    }
}
