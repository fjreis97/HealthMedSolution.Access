﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class LoginRequest
{
    public string Input { get; set; }
    public string Password { get; set; }
}
