using Microsoft.AspNetCore.Mvc;
using FoodOrder.Models.Interfaces;

namespace FoodOrder.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepo;

        public CartBadgeViewComponent(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int count = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                var items = await _cartRepo.GetCartByUserAsync(userId);
                count = items?.Count() ?? 0;
            }

            return View(count);
        }
    }
}
