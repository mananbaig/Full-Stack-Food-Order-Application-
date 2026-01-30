using FoodOrder.Models;
using FoodOrder.Models.Interfaces;
using FoodOrder.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodOrder.Controllers
{
    /*[Authorize(Roles = "Admin")]*/

    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IOrderRepository _orderRepo;

        public AdminController(
            IFoodRepository foodRepo,
            IOrderRepository orderRepo)
        {
            _foodRepo = foodRepo;
            _orderRepo = orderRepo;
        }

        // Dashboard
        public IActionResult Index()
        {
            ViewBag.TotalFoods = _foodRepo.GetCount();
            ViewBag.TotalOrders = _orderRepo.GetCount();

            return View();
        }

        // List all foods
        public async Task<IActionResult> FoodList()
        {
            var foods = await _foodRepo.GetAllAsync();
            return View(foods.ToList());
        }

        // Add Food GET
        public IActionResult AddFood()
        {
            return View();
        }

        // Add Food POST
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> AddFood(FoodItem food)
        {
            if (!ModelState.IsValid)
                return View(food);

            await _foodRepo.AddAsync(food);
            TempData["Success"] = "Food item added successfully!";
            return RedirectToAction(nameof(FoodList));
        }

        public async Task<IActionResult> EditFood(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);

            if (food == null)
                return NotFound();

            ViewData["Title"] = "Edit Food Item";
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFood(FoodItem food)
        {
            if (!ModelState.IsValid)
                return View(food);

            await _foodRepo.UpdateAsync(food);
            TempData["Success"] = "Food item updated successfully!";
            return RedirectToAction(nameof(FoodList));
        }

        // Delete Food
        public async Task<IActionResult> DeleteFood(int id)
        {
            await _foodRepo.DeleteAsync(id);
            TempData["Success"] = "Food item deleted successfully!";
            return RedirectToAction(nameof(FoodList));
        }

        // View Orders
        /*public IActionResult Orders()
        {
            var orders = _orderRepo.GetAllWithUsers();
            return View(orders);
        }*/
    }
}
