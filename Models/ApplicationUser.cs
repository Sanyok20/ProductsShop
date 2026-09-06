using Microsoft.AspNetCore.Identity;

namespace Products.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
    }
}