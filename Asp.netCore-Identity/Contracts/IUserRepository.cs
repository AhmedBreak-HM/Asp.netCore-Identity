namespace Asp.netCore_Identity.Contracts
{
    using Asp.netCore_Identity.Dtos;
    using Asp.netCore_Identity.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task<IReadOnlyList<User>> GetUsers();

        Task<User> GetUserById(int id);

        Task<IReadOnlyList<UserForReturnDto>> GetUsersWithRole();

        Task<UserForReturnDto> GetUserByIdWithRole(int id);
    }
}
