using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUserAsync(RegisterUserModel userModel);
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> DeleteUserAsync(Guid id);
        Task<UserModel> UpdateUserAsync(Guid id, UserModel userModel);
        Task<LoginResponseModel> LoginUserAsync(LoginModel loginModel);
        Task<UserModel> UpdateEmailAddressAsync(UserModel userModel);
        Task<UserModel> UpdatePasswordAsync(Guid id, string oldPass, string newPass);
        Task<TokenModel> GenerateNewTokensAsync(Guid id, string refreshToken);
        Task<bool> CheckIfUserExistsByEmailAsync(string email);
    }
}
