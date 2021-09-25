using Asp.netCore_Identity.Dtos;
using Asp.netCore_Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCore_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserForReturnDto>>> GetUsers()
        {
            var result = await _userManager.Users.ToListAsync();
            var userToReturn = _mapper.Map<List<UserForReturnDto>>(result);
            return Ok(userToReturn);
        }


        [HttpGet("{id}", Name = "GetUserById")]
        //[HttpGet("GetUserById/{id}")]

        public async Task<ActionResult<UserForReturnDto>> GetUserById(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            var userToReturn = _mapper.Map<UserForReturnDto>(user);

            return Ok(userToReturn);
        }

    }
}
