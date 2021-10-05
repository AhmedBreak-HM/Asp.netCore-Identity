namespace Asp.netCore_Identity.Repositories
{
    using Asp.netCore_Identity.Contracts;
    using Asp.netCore_Identity.Dtos;
    using Asp.netCore_Identity.Models;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManger;

        private readonly RoleManager<Role> _roleManager;

        private readonly IMapper _mapper;

        public UserRepository(UserManager<User> userManger, IMapper mapper,
                              RoleManager<Role> roleManager)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<User> GetUserById(int id)
        {
            var userFromRepo = await _userManger.Users.FirstOrDefaultAsync(u => u.Id == id);

            return userFromRepo;
        }

        public async Task<UserForReturnDto> GetUserByIdWithRole(int id)
        {
            var user = await _userManger.Users.FirstOrDefaultAsync(u => u.Id == id);
            var role = await _userManger.GetRolesAsync(user);
            var userWithRole = new UserForReturnDto
            {
                Id = user.Id,
                UserName = user.UserName,
                NormalizedUserName = user.NormalizedUserName,
                RolesString = role.ToList()
            };

            return userWithRole;
        }

        public async Task<IReadOnlyList<User>> GetUsers()
        {
            return await _userManger.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<UserForReturnDto>> GetUsersWithRole()
        {
            var userRole = await (from user in _userManger.Users
                                  orderby user.Id
                                  select new UserForReturnDto
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      NormalizedUserName = user.NormalizedUserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in _roleManager.Roles
                                               on userRole.RoleId equals role.Id
                                               select new RoleForUserDto { Name = role.Name }
                                       ).ToList()
                                  }
                                  ).ToListAsync();

            return userRole;
        }
    }
}
