using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Asp.netCore_Identity.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}