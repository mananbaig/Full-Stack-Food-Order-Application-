using FoodOrder.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _CartRepo;
        public CartController(ICartRepository cartRepository)
        {
            _CartRepo = cartRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Cart()
        {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var items = await _CartRepo.GetCartByUserAsync(userId);
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            await _CartRepo.RemoveFromCartAsync(cartId);
            return RedirectToAction("Cart");
        }
    }
}
