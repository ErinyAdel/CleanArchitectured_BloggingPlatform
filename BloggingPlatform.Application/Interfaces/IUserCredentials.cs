﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IUserCredentials
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
