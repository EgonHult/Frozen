using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersAsync()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return Ok(result);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUser(UserModel user)
        {
            if (user != null)
            {
                try
                {
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

        // DELETE: api/Users/5
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

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
