using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IServiceService _serviceService;
        private readonly IMaterialRepository _materialRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IServiceService serviceService,
            IMaterialRepository materialRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _serviceService = serviceService;
            _materialRepository = materialRepository;
        }

        //public async Task AddOrderDetailsAsync(List<OrderDetail> orderDetails)
        //{
        //    foreach (var orderDetail in orderDetails)
        //    {

        //        // Kiểm tra Material
        //        if (orderDetail.MaterialId.HasValue)
        //        {
        //            var material = await _materialRepository.GetMaterialByIdAsync(orderDetail.MaterialId.Value);
        //            if (material == null || material.Status != StatusConstants.AVAILABLE)
        //            {
        //                throw new InvalidOperationException("Vật liệu được chỉ định không có sẵn hoặc không hoạt động.");
        //            }
        //        }
        //        // Kiểm tra Service
        //        var service = await _serviceService.GetServiceByIdAsync((int)orderDetail.ServiceId!);
        //        if (service == null || service.Status != StatusConstants.AVAILABLE)
        //        {
        //            throw new InvalidOperationException("Dịch vụ được chỉ định không có sẵn hoặc không hoạt động.");
        //        }
        //        // Kiểm tra quantity
        //        //if (orderDetail.Quantity <= 0)
        //        //{
        //        //    throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
        //        //}

        //        // Kiểm tra price
        //        if (orderDetail.Price <= 0)
        //        {
        //            throw new InvalidOperationException("Giá phải lớn hơn 0.");
        //        }
        //    }

        //    await _orderDetailRepository.AddOrderDetailsAsync(orderDetails);
        //}

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {

            // Kiểm tra Material
            //if (orderDetail.MaterialId.HasValue)
            //{
            //    var material = await _materialRepository.GetMaterialByIdAsync(orderDetail.MaterialId.Value);
            //    if (material == null || material.Status != StatusConstants.AVAILABLE)
            //    {
            //        throw new InvalidOperationException("Vật liệu được chỉ định không có sẵn hoặc không hoạt động.");
            //    }
            //    orderDetail.Price += material.Price;
            //}
            if (orderDetail.ServiceId.HasValue)
            {
                var service = await _serviceService.GetServiceByIdAsync(orderDetail.ServiceId.Value);
                if (service == null || service.Status != StatusConstants.AVAILABLE)
                {
                    throw new InvalidOperationException("Dịch vụ được chỉ định không có sẵn hoặc không hoạt động.");
                }
                orderDetail.Price += await _serviceService.GetServiceFinalPriceAsync(orderDetail.ServiceId.Value);
            }

            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            await _orderDetailRepository.DeleteOrderDetailAsync(id);
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            return await _orderDetailRepository.GetOrderDetailByIdAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public async Task UpdateOrderDetailAsync(int existingOrderDetailId, OrderDetail orderDetail)
        {
            var existingOrderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(existingOrderDetailId);
            if (existingOrderDetail == null)
            {
                throw new InvalidOperationException($"Không tìm thấy Order Detail với id: {existingOrderDetailId}");
            }
            if(orderDetail.ProcessState != null)
            {
                existingOrderDetail.ProcessState = orderDetail.ProcessState;
            }
            if(orderDetail.AssetUrls != null)
            {
                existingOrderDetail.AssetUrls = orderDetail.AssetUrls;
            }
            await _orderDetailRepository.UpdateOrderDetailAsync(existingOrderDetail);
        }

    }
}
