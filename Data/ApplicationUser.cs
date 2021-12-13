using Microsoft.AspNetCore.Identity;

namespace the_kitchen_aspnet_core.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
