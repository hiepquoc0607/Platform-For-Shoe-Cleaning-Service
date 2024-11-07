using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.OrderDetail;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Library.Utils.Utils;
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

        public async Task AddOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                // Kiểm tra MaterialId và ServiceId
                //if (!orderDetail.MaterialId.HasValue && !orderDetail.ServiceId)
                //{
                //    throw new InvalidOperationException("Phải cung cấp ít nhất một trong hai: MaterialId hoặc ServiceId.");
                //}

                //if (orderDetail.MaterialId.HasValue && orderDetail.ServiceId)
                //{
                //    throw new InvalidOperationException("Chỉ được cung cấp một trong hai: MaterialId hoặc ServiceId.");
                //}

                // Kiểm tra Material
                if (orderDetail.MaterialId.HasValue)
                {
                    var material = await _materialRepository.GetMaterialByIdAsync(orderDetail.MaterialId.Value);
                    if (material == null || material.Status != StatusConstants.Available)
                    {
                        throw new InvalidOperationException("Vật liệu được chỉ định không có sẵn hoặc không hoạt động.");
                    }
                }
                // Kiểm tra Service
                //else if (orderDetail.ServiceId)
                //{
                //    var service = await _serviceService.GetServiceByIdAsync(orderDetail.ServiceId);
                //    if (service == null || service.Status != StatusConstants.Available)
                //    {
                //        throw new InvalidOperationException("Dịch vụ được chỉ định không có sẵn hoặc không hoạt động.");
                //    }
                //}
                // Kiểm tra quantity
                if (orderDetail.Quantity <= 0)
                {
                    throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
                }

                // Kiểm tra price
                if (orderDetail.Price <= 0)
                {
                    throw new InvalidOperationException("Giá phải lớn hơn 0.");
                }

                // Kiểm tra status
                //if (String.IsNullOrEmpty(orderDetail) || !Util.IsValidOrderDetailStatus(orderDetail))
                //{
                //    throw new InvalidOperationException("Trạng thái không hợp lệ.");
                //}

                //orderDetail.Status = orderDetail.ToUpper();
            }

            await _orderDetailRepository.AddOrderDetailsAsync(orderDetails);
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            //if (!orderDetail.MaterialId.HasValue && !orderDetail.ServiceId.HasValue)
            //{
            //    throw new InvalidOperationException("Phải cung cấp ít nhất một trong hai: MaterialId hoặc ServiceId.");
            //}

            //if (orderDetail.MaterialId.HasValue && orderDetail.ServiceId.HasValue)
            //{
            //    throw new InvalidOperationException("Chỉ được cung cấp một trong hai: MaterialId hoặc ServiceId.");
            //}

            // Kiểm tra Material
            if (orderDetail.MaterialId.HasValue)
            {
                var material = await _materialRepository.GetMaterialByIdAsync(orderDetail.MaterialId.Value);
                if (material == null || material.Status != StatusConstants.Available)
                {
                    throw new InvalidOperationException("Vật liệu được chỉ định không có sẵn hoặc không hoạt động.");
                }
            }
            // Kiểm tra Service
            //else if (orderDetail.ServiceId.)
            //{
            //    var service = await _serviceService.GetServiceByIdAsync(orderDetail.ServiceId.Value);
            //    if (service == null || service.Status != StatusConstants.Available)
            //    {
            //        throw new InvalidOperationException("Dịch vụ được chỉ định không có sẵn hoặc không hoạt động.");
            //    }
            //}
            // Kiểm tra quantity
            if (orderDetail.Quantity <= 0)
            {
                throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
            }

            orderDetail.Price = await _serviceService.GetServiceFinalPriceAsync(orderDetail.ServiceId!);


            //orderDetail.Status = StatusConstants.PROCESSING;

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

        public async Task UpdateOrderDetailAsync(OrderDetailRequest request, int existingOrderDetailId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(OrderDetailRequest), "Chi tiết đơn hàng không thể là null.");
            }

            var existingOrderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(existingOrderDetailId);
            if (existingOrderDetail == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {existingOrderDetailId}.");
            }

            // Kiểm tra quantity
            if (request.Quantity.HasValue && request.Quantity.Value <= 0)
            {
                throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
            }

            if (!string.IsNullOrEmpty(request.Status) && !Util.IsValidOrderDetailStatus(request.Status))
            {
                throw new InvalidOperationException("Trạng thái không hợp lệ.");
            }

            if (request.Quantity.HasValue)
            {
                existingOrderDetail.Quantity = request.Quantity.Value;
            }
            //if (!string.IsNullOrEmpty(request.Status) && Util.IsValidOrderDetailStatus(request.Status))
            //{
            //    existingOrderDetail.Status = request.Status.Trim().ToUpper();
            //}
            await _orderDetailRepository.UpdateOrderDetailAsync(existingOrderDetail);
        }

    }
}
