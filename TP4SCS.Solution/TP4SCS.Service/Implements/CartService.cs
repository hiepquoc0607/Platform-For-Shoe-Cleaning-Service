using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IServiceService _serviceService;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public CartService(ICartRepository cartRepository, IServiceService serviceService
            , ICartItemRepository cartItemRepository, IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository)
        {
            _cartRepository = cartRepository;
            _serviceService = serviceService;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task ClearCartAsync(int cartId)
        {
            await _cartRepository.ClearCartAsync(cartId);
        }

        public async Task<Cart> CreateCartAsync(int userId)
        {
            return await _cartRepository.CreateCartAsync(userId);
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<decimal> GetCartTotalAsync(int cartId)
        {
            Cart? cart = await _cartRepository.GetCartByIdAsync(cartId);
            decimal totalPrice = 0;

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return totalPrice;
            }

            foreach (var item in cart.CartItems)
            {
                decimal servicePrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId!.Value);

                totalPrice += servicePrice * item.Quantity;
            }

            return totalPrice;
        }
        public async Task CheckoutAsync(CheckoutRequest request)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByIdsAsync(request.CartItemIds);
            var groupedItems = cartItems
                .GroupBy(item => item.BranchId)
                .Select(group => new
                {
                    BranchId = group.Key,
                    Items = group.ToList()
                });

            var orders = new List<Order>();

            foreach (var group in groupedItems)
            {
                var order = new Order
                {
                    AccountId = request.AccountId,
                    AddressId = request.AddressId,
                    CreateTime = DateTime.UtcNow,
                    IsAutoReject = request.IsAutoReject,
                    Note = request.Note,
                    Status = StatusConstants.PENDING,
                    ShippingUnit = request.IsShip ? "Giao Hàng Nhanh" : null,
                    ShippingCode = request.IsShip ? "" : null,
                    DeliveredFee = request.IsShip ? 1 : 0,
                    OrderDetails = new List<OrderDetail>()
                };

                decimal orderPrice = 0;

                foreach (var item in group.Items)
                {
                    var finalPrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId!.Value);

                    order.OrderDetails.Add(new OrderDetail
                    {
                        BranchId = item.BranchId,
                        ServiceId = item.ServiceId,
                        Quantity = item.Quantity,
                        Price = finalPrice,
                        Status = StatusConstants.PENDING
                    });

                    orderPrice += finalPrice * item.Quantity;
                }

                order.OrderPrice = orderPrice;
                order.TotalPrice = orderPrice + 0;
                orders.Add(order);
            }

            await _orderRepository.AddOrdersAsync(orders);
            await _cartItemRepository.RemoveItemsFromCartAsync(request.CartItemIds);
        }
    }
}
