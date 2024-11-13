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
                    CreateTime = DateTime.Now,
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
        public async Task CheckoutForCartAsync(HttpClient httpClient, CheckoutCartRequest request)
        {
            var orders = new List<Order>();

            foreach (var cart in request.Carts)
            {
                // Lấy các CartItems từ CartCheckout hiện tại
                var cartItems = await _cartItemRepository.GetCartItemsByIdsAsync(cart.CartItemIds);

                // Nhóm các CartItems theo BranchId
                var groupedItems = cartItems
                    .GroupBy(item => item.BranchId)
                    .Select(group => new
                    {
                        BranchId = group.Key,
                        Items = group.ToList()
                    });

                foreach (var group in groupedItems)
                {
                    var order = new Order
                    {
                        AccountId = request.AccountId,
                        AddressId = request.AddressId,
                        CreateTime = DateTime.Now,
                        IsAutoReject = request.IsAutoReject,
                        Note = cart.Note,
                        Status = StatusConstants.PENDING,
                        ShippingUnit = cart.IsShip ? "Giao Hàng Nhanh" : null,
                        ShippingCode = cart.IsShip ? "" : null,
                        OrderDetails = new List<OrderDetail>()
                    };

                    decimal orderPrice = 0;
                    int quantity = 0;

                    foreach (var item in group.Items)
                    {
                        decimal finalPrice = 0;
                        if (item.ServiceId.HasValue)
                        {
                            finalPrice = await _serviceService.GetServiceFinalPriceAsync(item.ServiceId.Value);
                        }

                        order.OrderDetails.Add(new OrderDetail
                        {
                            BranchId = item.BranchId,
                            ServiceId = item.ServiceId.Value,
                            Quantity = item.Quantity,
                            Price = finalPrice,
                        });

                        orderPrice += finalPrice * item.Quantity;
                        quantity += item.Quantity;
                    }

                    order.DeliveredFee = cart.IsShip ? await GetFeeShip(httpClient, request.AddressId!.Value, group.BranchId, quantity) : 0;
                    order.OrderPrice = orderPrice;
                    order.PendingTime = DateTime.Now;
                    order.TotalPrice = orderPrice + order.DeliveredFee;

                    orders.Add(order);
                }
            }

            await _orderRepository.AddOrdersAsync(orders);

            // Xóa các CartItem đã xử lý
            var allCartItemIds = request.Carts.SelectMany(c => c.CartItemIds).ToArray();
            if (allCartItemIds.Any())
            {
                await _cartItemRepository.RemoveItemsFromCartAsync(allCartItemIds);
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
                throw new KeyNotFoundException($"Không tìm thấy địa chỉ của tài khoản với ID: {addressId}");
            }

            var getFeeShipRequest = new GetShipFeeRequest
            {
                FromDistricId = branch.DistrictId,
                FromWardCode = branch.WardCode,
                ToDistricId = address.DistrictId,
                ToWardCode = address.WardCode,
            };

            // Tính phí ship cho 1 hộp
            var baseFeeShip = await _shipService.GetShippingFeeAsync(httpClient, getFeeShipRequest);

            // Tính phí ship theo số lượng
            decimal totalFeeShip;
            if (quantity <= 5)
            {
                totalFeeShip = baseFeeShip;
            }
            else if (quantity <= 10)
            {
                totalFeeShip = baseFeeShip * 1.2m; // tăng 20%
            }
            else
            {
                totalFeeShip = baseFeeShip * 1.5m; // tăng 50%
            }

            return totalFeeShip;
        }


    }
}
