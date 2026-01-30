using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Controllers
{
    public class OrderController : Controller
    {
        private readonly FoodOrder.Models.Interfaces.IOrderRepository _orderRepo;
        private readonly FoodOrder.Models.Interfaces.IOrderItemRepository _orderItemRepo;
        private readonly FoodOrder.Models.Interfaces.ICartRepository _cartRepo;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<FoodOrder.Hubs.NotificationHub> _hubContext;

        public OrderController(FoodOrder.Models.Interfaces.IOrderRepository orderRepo,
            FoodOrder.Models.Interfaces.IOrderItemRepository orderItemRepo,
            FoodOrder.Models.Interfaces.ICartRepository cartRepo,
            Microsoft.AspNetCore.SignalR.IHubContext<FoodOrder.Hubs.NotificationHub> hubContext)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _cartRepo = cartRepo;
            _hubContext = hubContext;
        }

        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }
        public IActionResult OrderHistory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var cartItems = (await _cartRepo.GetCartByUserAsync(userId)).ToList();
            if (cartItems == null || !cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Cart", "Cart");
            }

            decimal total = cartItems.Sum(c => c.Price * c.Quantity);

            var order = new FoodOrder.Models.Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = total,
                Status = "Pending"
            };

            int orderId = await _orderRepo.CreateOrderAsync(order);

            var orderItems = cartItems.Select(c => new FoodOrder.Models.OrderItem
            {
                OrderId = orderId,
                FoodId = c.FoodId,
                Quantity = c.Quantity,
                UnitPrice = c.Price
            }).ToList();

            await _orderItemRepo.AddOrderItemAsync(orderItems);

            // clear cart
            foreach (var c in cartItems)
            {
                await _cartRepo.RemoveFromCartAsync(c.CartId);
            }

            // send a basic notification to connected clients
            try
            {
                // Notify only admins in real time
                var adminsGroup = FoodOrder.Hubs.NotificationHub.GetAdminsGroupName();
                await _hubContext.Clients.Group(adminsGroup).SendCoreAsync("ReceiveNotification", new object[] { $"New order placed (#{orderId})" });

                // Notify all clients to refresh cart counts (optional)
                await _hubContext.Clients.All.SendCoreAsync("CartUpdated", new object[] { 0 });
            }
            catch { }

            return RedirectToAction("OrderConfirmation");
        }

    }
}
