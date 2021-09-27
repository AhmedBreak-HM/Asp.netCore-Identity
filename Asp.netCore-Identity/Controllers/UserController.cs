using Asp.netCore_Identity.Dtos;
using Asp.netCore_Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCore_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager,
                              IMapper mapper, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        //[HttpGet("GetUserById/{id}")]

        public async Task<ActionResult<UserForReturnDto>> GetUserById(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            var userToReturn = _mapper.Map<UserForReturnDto>(user);


            var roles = await _userManager.GetRolesAsync(user);

            var userRole = new UserForReturnDto { 
                                Id= user.Id,UserName = user.UserName,
                                NormalizedUserName = user.NormalizedUserName,
                                Roles =roles.ToList() };

            //var userToReturn = _mapper.Map<UserForReturnDto>(userRole);

            return Ok(userRole);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserForReturnDto>>> GetUsers()
        {
            var userWithRole = await (from user in _userManager.Users
                                      orderby user.Id
                                      select new 
                                      {
                                          Id = user.Id,
                                          UserName = user.UserName,
                                          Roles = (from roleuser in user.UserRoles
                                                   join role in _roleManager.Roles on
                                                   roleuser.RoleId equals role.Id
                                                   select new 
                                                   {
                                                       Name = role.Name
                                                   }).ToList()
                                      }
                                     ).ToListAsync();

            return Ok(userWithRole);
        }
    }
}