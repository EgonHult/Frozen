using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(UserModel userModel);
    }
}
