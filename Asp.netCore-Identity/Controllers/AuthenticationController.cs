using Asp.netCore_Identity.Dtos;
using Asp.netCore_Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Asp.netCore_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthenticationController(UserManager<User> userManager,
            IConfiguration configuration, IMapper mapper, SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }




        [HttpPost("signUp")]
        public async Task<ActionResult<UserForReturnDto>> SignUp(UserForCreateDto userForCreateDto)
        {
            var userToCreate = _mapper.Map<User>(userForCreateDto);

            var result = await _userManager.CreateAsync(userToCreate, userForCreateDto.Password);


            if (!result.Succeeded) return BadRequest(result.Errors);

            // Create Role Member For User
            var user = await _userManager.FindByNameAsync(userForCreateDto.UserName);
            var resultRoleCreate = await _userManager.AddToRolesAsync(user, new[] { "Member" });
            if (!resultRoleCreate.Succeeded) return BadRequest(result.Errors);
            // ----------------------------

            var userWithRole = await _userManager.Users.Where(u => u.UserName == userForCreateDto.UserName)
                                  .Include(u => u.UserRoles).ThenInclude(ur => ur.Role.Name)
                                  .FirstOrDefaultAsync();

            var userToReturn = _mapper.Map<UserForReturnDto>(userWithRole);


            return CreatedAtAction("GetUserById", new { controller = "User", id = userToCreate.Id }, userToReturn);
            //return CreatedAtRoute("GetUserById", new { controller = "User", id = userToCreate.Id }, userToReturn);
        }


        [HttpPost("logIn")]
        public async Task<ActionResult<UserForReturnDto>> LogIn(UserForLogInDto userForLogInDto)
        {
            var userFromRepo = await _userManager.FindByNameAsync(userForLogInDto.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(userFromRepo, userForLogInDto.Password, false);

            if (result.Succeeded)
            {
                var userApp = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userForLogInDto.UserName.ToUpper());

                var userToReturn = _mapper.Map<UserForReturnDto>(userApp);

                return Ok(new
                {
                    token = GenerateJwtToken(userApp),
                    userToReturn
                });
            }

            return Unauthorized();

        }


        private async Task<string> GenerateJwtToken(User user)
        {

            // Create Token

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var roles = await GetUserRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // genreated key and convert to bytes
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:TokenKey").Value));

            // genreated Credentials by hashing key
            var Creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // genreate Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = Creds
            };

            // genreate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<IList<string>> GetUserRolesAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
