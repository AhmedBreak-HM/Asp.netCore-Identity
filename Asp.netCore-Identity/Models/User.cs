using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Asp.netCore_Identity.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}