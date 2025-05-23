﻿using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Interfaces;

public interface ITokenService
{
    public Task<string> GenerateTokenCollaborator(LoginRequest requestInitial);
    public Task<string> GenerateTokenPatient(LoginRequest requestInitial);

}
