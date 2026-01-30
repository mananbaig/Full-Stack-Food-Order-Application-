using FoodOrder.Models;       // Your model namespace
using FoodOrder.Models.Interfaces;
using FoodOrder.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrder.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<IActionResult> DashBoard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var model = new DashboardViewModel
            {
                TotalOrders = await _userRepo.GetTotalOrdersAsync(userId),
                TotalSpent = await _userRepo.GetTotalSpentAsync(userId),
                TotalItemsOrdered = await _userRepo.GetTotalItemsOrderedAsync(userId),
                PendingOrders = await _userRepo.GetPendingOrdersAsync(userId)
            };

            return View(model);
        }
    }
}
