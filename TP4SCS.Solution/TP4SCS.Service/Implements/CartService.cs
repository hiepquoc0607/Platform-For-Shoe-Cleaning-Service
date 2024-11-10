using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Cart;
using TP4SCS.Library.Models.Request.ShipFee;
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
        private readonly IShipService _shipService;
        private readonly IAddressRepository _addressRepository;
        private readonly IBranchRepository _branchRepository;

        public CartService(ICartRepository cartRepository, IServiceService serviceService
            , ICartItemRepository cartItemRepository, IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IShipService shipService,
            IAddressRepository addressRepository,
            IBranchRepository branchRepository)
        {
            _cartRepository = cartRepository;
            _serviceService = serviceService;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _shipService = shipService;
            _addressRepository = addressRepository;
            _branchRepository = branchRepository;
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
        public async Task CheckoutForServiceAsync(HttpClient httpClient, CheckoutForServiceRequest request)
        {
            IEnumerable<dynamic> groupedItems = Enumerable.Empty<dynamic>();

            groupedItems = request.Items
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
                    OrderDetails = new List<OrderDetail>()
                };

                decimal orderPrice = 0;
                int quantiy = 0;

                foreach (var item in group.Items)
                {
                    var finalPrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId);

                    order.OrderDetails.Add(new OrderDetail
                    {
                        BranchId = item.BranchId,
                        ServiceId = item.ServiceId,
                        Quantity = item.Quantity,
                        Price = finalPrice,
                    });

                    orderPrice += finalPrice * item.Quantity;
                    quantiy += item.Quantity;
                }
                order.DeliveredFee = request.IsShip ? (await GetFeeShip(httpClient, request.AddressId!.Value, group.BranchId, quantiy)) : 0;
                order.OrderPrice = orderPrice;
                order.PendingTime = DateTime.UtcNow;
                order.CreateTime = DateTime.UtcNow;
                order.TotalPrice = orderPrice +
                    (request.IsShip ? (await GetFeeShip(httpClient, request.AddressId!.Value, group.BranchId, quantiy)) : 0);
                orders.Add(order);
            }

            await _orderRepository.AddOrdersAsync(orders);
        }
        public async Task CheckoutForCartItemAsync(HttpClient httpClient, CheckoutForCartItemRequest request)
        {
            IEnumerable<dynamic> groupedItems = Enumerable.Empty<dynamic>();

            var cartItems = await _cartItemRepository.GetCartItemsByIdsAsync(request.CartItemIds);
            groupedItems = cartItems
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
                    OrderDetails = new List<OrderDetail>()
                };

                decimal orderPrice = 0;
                int quantiy = 0;

                foreach (var item in group.Items)
                {
                    var finalPrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId);

                    order.OrderDetails.Add(new OrderDetail
                    {
                        BranchId = item.BranchId,
                        ServiceId = item.ServiceId,
                        Quantity = item.Quantity,
                        Price = finalPrice,
                    });

                    orderPrice += finalPrice * item.Quantity;
                    quantiy += item.Quantity;
                }
                order.DeliveredFee = request.IsShip ? (await GetFeeShip(httpClient, request.AddressId!.Value, group.BranchId, quantiy)) : 0;
                order.OrderPrice = orderPrice;
                order.PendingTime = DateTime.UtcNow;
                order.CreateTime = DateTime.UtcNow;
                order.TotalPrice = orderPrice +
                    (request.IsShip ? (await GetFeeShip(httpClient, request.AddressId!.Value, group.BranchId, quantiy)) : 0);
                orders.Add(order);
            }

            await _orderRepository.AddOrdersAsync(orders);
            if (request.CartItemIds.Any())
            {
                await _cartItemRepository.RemoveItemsFromCartAsync(request.CartItemIds);
            }
        }

        public async Task<decimal> GetFeeShip(HttpClient httpClient, int addressId, int branchId, int quantity)
        {
            var address = await _addressRepository.GetByIDAsync(addressId);
            var branch = await _branchRepository.GetBranchByIdAsync(branchId);

            if (branch == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chi nhánh với ID: {branchId}");
            }
            if (address == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy địa chỉ của tài khoảng với ID: {addressId}");
            }

            var getFeeShipReqeust = new GetShipFeeRequest
            {
                FromDistricId = branch.DistrictId,
                FromWardCode = branch.WardCode,
                ToDistricId = address.DistrictId,
                ToWardCode = address.WardCode,
            };
            return await _shipService.GetShippingFeeAsync(httpClient, getFeeShipReqeust, quantity);
        }

    }
}
