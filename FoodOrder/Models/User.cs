using Microsoft.AspNetCore.Identity;

namespace FoodOrder.Models
{
    public class User : IdentityUser
    {
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}



