using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FoodOrder.Hubs
{
    public class NotificationHub : Hub
    {
        private const string AdminsGroup = "Admins";

        public override async Task OnConnectedAsync()
        {
            // If the connected user is an admin (simple email check), add to Admins group
            var email = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email) && email.ToLowerInvariant() == "admin@gmail.com")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            // Remove from group on disconnect (safe even if not present)
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminsGroup);
            await base.OnDisconnectedAsync(exception);
        }

        // Server can call this to broadcast to all admins
        public static string GetAdminsGroupName() => AdminsGroup;
    }
}
