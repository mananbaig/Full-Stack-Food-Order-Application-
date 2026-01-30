using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        public NotificationsViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            // Start with empty list; client JS will populate via SignalR
            var notifications = new List<string>();
            return View(notifications);
        }
    }
}
