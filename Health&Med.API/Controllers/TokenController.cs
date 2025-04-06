using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

public class TokenController(ITokenService _tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> AuthenticationCollaborator([FromBody] LoginRequest request)
    {
        var token = await _tokenService.GenerateTokenCollaborator(request);

        if (!string.IsNullOrWhiteSpace(token))
            return Ok(token);

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> AuthenticationPatient([FromBody] LoginRequest request)
    {
        var token = await _tokenService.GenerateTokenPatient(request);

        if (!string.IsNullOrWhiteSpace(token))
            return Ok(token);

        return Unauthorized();
    }
}
