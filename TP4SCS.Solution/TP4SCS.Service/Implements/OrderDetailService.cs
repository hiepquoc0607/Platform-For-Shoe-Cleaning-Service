using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Utils;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMaterialRepository _materialRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IServiceRepository serviceRepository,
            IMaterialRepository materialRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _serviceRepository = serviceRepository;
            _materialRepository = materialRepository;
        }

        public async Task AddOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                // Kiểm tra MaterialId và ServiceId
                if (!orderDetail.MaterialId.HasValue && !orderDetail.ServiceId.HasValue)
                {
                    throw new InvalidOperationException("Phải cung cấp ít nhất một trong hai: MaterialId hoặc ServiceId.");
                }

                if (orderDetail.MaterialId.HasValue && orderDetail.ServiceId.HasValue)
                {
                    throw new InvalidOperationException("Chỉ được cung cấp một trong hai: MaterialId hoặc ServiceId.");
                }

                // Kiểm tra Material
                if (orderDetail.MaterialId.HasValue)
                {
                    var material = await _materialRepository.GetMaterialByIdAsync(orderDetail.MaterialId.Value);
                    if (material == null || material.Status != StatusConstants.Active)
                    {
                        throw new InvalidOperationException("Vật liệu được chỉ định không có sẵn hoặc không hoạt động.");
                    }
                }
                // Kiểm tra Service
                else if (orderDetail.ServiceId.HasValue)
                {
                    var service = await _serviceRepository.GetServiceByIdAsync(orderDetail.ServiceId.Value);
                    if (service == null || service.Status != StatusConstants.Active)
                    {
                        throw new InvalidOperationException("Dịch vụ được chỉ định không có sẵn hoặc không hoạt động.");
                    }
                }

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
                if (!Util.IsValidGeneralStatus(orderDetail.Status))
                {
                    throw new InvalidOperationException("Trạng thái không hợp lệ.");
                }

                orderDetail.Status = orderDetail.Status.ToUpper();
            }

            await _orderDetailRepository.AddOrderDetailsAsync(orderDetails);
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

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail, int existingOrderDetailId)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail), "Chi tiết đơn hàng không thể là null.");
            }

            var existingOrderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(existingOrderDetailId);
            if (existingOrderDetail == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy OrderDetail với ID {existingOrderDetailId}.");
            }

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
            if (!Util.IsValidGeneralStatus(orderDetail.Status))
            {
                throw new InvalidOperationException("Trạng thái không hợp lệ.");
            }

            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.Price = orderDetail.Price;
            existingOrderDetail.Status = orderDetail.Status.ToUpper();

            // Lưu thay đổi
            await _orderDetailRepository.UpdateOrderDetailAsync(existingOrderDetail);
        }

    }
}
