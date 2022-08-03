using Microsoft.AspNetCore.Identity;

namespace _02082022_SignalRProject.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public string ConnectId { get; set; }
        public bool isOnline { get; set; }
    }
}
