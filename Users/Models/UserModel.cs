﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Models
{
    public class UserModel : RegisterUserModel
    {
        public Guid Id { get; set; }
    }
}
