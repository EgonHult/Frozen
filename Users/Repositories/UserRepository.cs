using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Context;
using Users.Models;
using Users.Services;

namespace Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly JwtTokenHandler _tokenHandler;

        public UserRepository(UserDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<AppRole> roleManager, JwtTokenHandler tokenHandler)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._tokenHandler = tokenHandler;
        }

        public async Task<UserModel> CreateUserAsync(RegisterUserModel userModel)
        {
            User user = new User()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                PhoneNumber = userModel.PhoneNumber,
                Address = userModel.Address,
                City = userModel.City,
                Email = userModel.Email,
                UserName = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            var role = await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded && role.Succeeded)
            {
                return await ConvertUserToUserModelAsync(user);
            }

            return null;
        }

        public async Task<UserModel> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    var deletedUser = await ConvertUserToUserModelAsync(user);
                    return deletedUser;
                }
                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var result = await _context.Users.Select(x => new UserModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                City = x.City,
                Zip = x.Zip
            }).ToListAsync();

            return result;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var userModel = await ConvertUserToUserModelAsync(user);
                return userModel;
            }

            return null;
        }

        public async Task<UserModel> ConvertUserToUserModelAsync(User user)
        {
            var userModel = new UserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                Zip = user.Zip
            };

            return await Task.FromResult(userModel);
        }

        public async Task<UserModel> UpdateUserAsync(Guid id, UserModel userModel)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (id == user.Id)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.PhoneNumber = userModel.PhoneNumber;
                user.City = userModel.City;
                user.Zip = userModel.Zip;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return userModel;
                }

                return null;
            }

            return null;
        }

        public async Task<LoginResponseModel> LoginUserAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Username);

            if (user == null)
                return null;

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if(signInResult.Succeeded)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                var token = _tokenHandler.CreateToken(user, isAdmin);
                var refreshToken = _tokenHandler.CreateRefreshToken(user);

                // Build response model with user object and tokens
                var responseModel = new LoginResponseModel()
                {
                    User = await ConvertUserToUserModelAsync(user),
                    Token = token,
                    RefreshToken = refreshToken
                };

                return responseModel;
            }

            return null;
        }

        public async Task<UserModel> UpdateEmailAddressAsync(UserModel userModel)
        {
            var emailAvailable = await _userManager.FindByEmailAsync(userModel.Email);

            // Emailaddress is available
            if (emailAvailable == null)
            {
                var user = await _userManager.FindByIdAsync(userModel.Id.ToString());
                user.Email = userModel.Email;
                user.UserName = userModel.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return userModel;
            }

            return null;
        }

        public async Task<UserModel> UpdatePasswordAsync(Guid id, string oldPass, string newPass)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, oldPass, newPass);

                if (result.Succeeded)
                    return await ConvertUserToUserModelAsync(user);
            }

            return null;
        }

        public async Task<TokenModel> GenerateNewTokensAsync(Guid id, string refreshToken)
        {
            var result = _tokenHandler.ValidateRefreshToken(refreshToken);

            if (result.Identity.IsAuthenticated)
            {
                // Get user and its role
                var user = await _userManager.FindByIdAsync(id.ToString());
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                var tokenModel = new TokenModel()
                {
                    Token = _tokenHandler.CreateToken(user, isAdmin),
                    RefreshToken = _tokenHandler.CreateRefreshToken(user)
                };

                return tokenModel;
            }

            return null;
        }
    }
}
