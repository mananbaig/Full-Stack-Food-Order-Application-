using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return Redirect("/Identity/Account/Login");
        }
        //[HttpPost]
        /* public IActionResult Login(User user)
         {

         }*/
        public IActionResult Register()
        {
            return Redirect("/Identity/Account/Register");
        }
        //[HttpPost]
        /* public IActionResult Register()
         {
             return View();
         }*/
    }
    
}
