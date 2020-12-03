using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }
    }
}
