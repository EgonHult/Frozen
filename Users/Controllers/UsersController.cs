using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Context;
using Users.Models;
using Users.Repositories;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IUserRepository _userRepository;

        public UsersController(UserDbContext context, IUserRepository userRepository)
        {
            _context = context;
            this._userRepository = userRepository;
        }

        // GET: api/Users
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersAsync()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return Ok(result);
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("getuserbyid/{id}")]
        public async Task<ActionResult<UserModel>> GetUser(Guid id)
        {
            var userExist = UserExists(id);

            if (!userExist)
                return NotFound();

            var result = await _userRepository.GetUserByIdAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserModel user)
        {
            if (id != user.Id)
                return BadRequest();

            try
            {
                var result = await _userRepository.UpdateUserAsync(id, user);
                if (result != null)
                    return Ok(result);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> PostUser(RegisterUserModel user)
        {
            if (user != null)
            {
                try
                {
                    var checkExisting = await _userRepository.CheckIfUserExistsByEmailAsync(user.Email);
                    if(checkExisting == true)
                    {
                        return Conflict();
                    }
                    var newUser = await _userRepository.CreateUserAsync(user);
                    if (newUser != null)
                        return Ok(newUser);
                }
                catch(Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        // POST: api/Users/login/
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> LoginUserAsync(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginUserAsync(loginModel);

                if (result == null)
                    return Unauthorized();

                return Ok(result);
            }

            return BadRequest();
        }


        // DELETE: api/Users/5
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(Guid id)
        {
            if(id != Guid.Empty)
            {
                var result = await _userRepository.DeleteUserAsync(id);

                if(result != null)
                    return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("token")]
        public async Task<ActionResult<TokenModel>> RequestNewTokenAsync(RenewTokenModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = UserExists(model.UserId);

                if (userExists)
                {
                    var tokenModel = await _userRepository.GenerateNewTokensAsync(model.UserId, model.Token);

                    if (tokenModel != null)
                        return Ok(tokenModel);
                }
                else
                    return NotFound();
            }

            return BadRequest();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
