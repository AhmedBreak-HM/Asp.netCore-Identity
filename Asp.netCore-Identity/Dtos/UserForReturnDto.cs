using Asp.netCore_Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCore_Identity.Dtos
{
    public class UserForReturnDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
