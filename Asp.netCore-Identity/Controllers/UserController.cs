using Asp.netCore_Identity.Contracts;
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
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<User> userManager,
                              IMapper mapper, RoleManager<Role> roleManager,
                              IUserRepository userRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        [HttpGet("{id}", Name = "GetUserById")]
        //[HttpGet("GetUserById/{id}")]

        public async Task<ActionResult<UserForReturnDto>> GetUserById(int id)
        {
            var userToReturn = await _userRepository.GetUserByIdWithRole(id);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserForReturnDto>>> GetUsers()
        {
            var usersRoles = await _userRepository.GetUsersWithRole();

            return Ok(usersRoles);
        }
    }
}