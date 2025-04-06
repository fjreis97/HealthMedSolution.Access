using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Enums;
using Health_Med.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

public class TokenService(IConfiguration _configuration, ICollaboratorService _collaboratorService, IPatientService _patientService) :ITokenService
{

    public async Task<string> GenerateTokenCollaborator(LoginRequest requestInitial)
    {
        try
        {
            var usuarioExistente = await _collaboratorService.VerifyExistence(requestInitial); 

            if (usuarioExistente == null)
                return string.Empty;

            //variável responsável por gerar o token
            var tokenHandler = new JwtSecurityTokenHandler();
            string roleName = Enum.GetName(typeof(Role), usuarioExistente.RoleId)!;
            //recuperando a chave secreta
            var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT")!);

            //propriedades do token
            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioExistente.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.NameIdentifier, usuarioExistente.Id.ToString()),
            new Claim(ClaimTypes.Name, usuarioExistente.Name),
            new Claim(ClaimTypes.Email, usuarioExistente.EmailAddress),
            new Claim(ClaimTypes.Role, roleName),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveCriptografia),
                                     SecurityAlgorithms.HmacSha256Signature)
            };

            //Cria o token 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<string> GenerateTokenPatient(LoginRequest requestInitial)
    {
        try
        {
            var usuarioExistente = await _collaboratorService.VerifyExistence(requestInitial);

            if (usuarioExistente == null)
                return string.Empty;

            //variável responsável por gerar o token
            var tokenHandler = new JwtSecurityTokenHandler();
            string roleName = Enum.GetName(typeof(Role), usuarioExistente.RoleId)!;
            //recuperando a chave secreta
            var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT")!);

            //propriedades do token
            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioExistente.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.NameIdentifier, usuarioExistente.Id.ToString()),
            new Claim(ClaimTypes.Name, usuarioExistente.Name),
            new Claim(ClaimTypes.Email, usuarioExistente.EmailAddress),
            new Claim(ClaimTypes.Role, roleName),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveCriptografia),
                                     SecurityAlgorithms.HmacSha256Signature)
            };

            //Cria o token 
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch
        {
            return string.Empty;
        }
    }
}
