using FoodOrder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using FoodOrder.Models.Repositories;
using FoodOrder.Models.Interfaces;

namespace FoodOrder.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IFoodRepository _foodRepository;
        private readonly ICartRepository _cartRepository;

        public MenuController(IFoodRepository foodRepository, ICartRepository cartRepository)
        {
            _foodRepository = foodRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Menu()
        {
            var items = await _foodRepository.GetAllAsync();
            return View(items.ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
           FoodItem ? foodItem =  await _foodRepository.GetByIdAsync(id);
            return View(foodItem);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            await _cartRepository.AddToCartAsync(userId, foodId, quantity);

            return RedirectToAction("Menu", "Menu");
        }

    }
}
