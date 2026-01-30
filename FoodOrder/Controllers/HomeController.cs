using System.Diagnostics;
using FoodOrder.Models;
using FoodOrder.Models.Interfaces;
using FoodOrder.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFoodRepository _foodRepository;

        public HomeController(ILogger<HomeController> logger, IFoodRepository foodRepository)
        {
            _logger = logger;
            _foodRepository = foodRepository;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _foodRepository.GetAllAsync();
            var list = items.ToList();

            var vm = new HomeIndexViewModel
            {
                Featured = list.Take(6),
                Latest = list.OrderByDescending(f => f.FoodId).Take(6)
            };

            return View(vm);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
