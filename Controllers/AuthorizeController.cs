using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
public class AuthorizeController : Controller
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly KeyMaterial _keyMaterial;

    public AuthorizeController (JwtSecurityTokenHandler handler, KeyMaterial material)
    {
        _tokenHandler = handler;
        _keyMaterial = material;
    }

    private IEnumerable<string> GetRoles (IEnumerable<Claim> rolesClaims)
    {
        foreach (var role in rolesClaims)
        {
            yield return role.Value;
        }
    }

    [HttpPost]
    public IActionResult Authorize ([FromBody] AuthRequest request)
    {
        try
        {
            if (!String.Equals(request.Username, "admin") && !String.Equals(request.Password, "admin"))
            {
                throw new UnauthorizedAccessException("Invalid Username or Password provided.");
            }

            var time = DateTime.UtcNow;

            var claims = new ClaimsIdentity(new List<Claim>{
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.Role, "Developer"),
            });

            /*
            Issuer and Audience should be changed to valid Urls or Names. 
             */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = time.AddHours(24),
                Subject = claims,
                IssuedAt = time,
                SigningCredentials = _keyMaterial.GetSigningCredentials(),
                Issuer = "localhost",
                Audience = "localhost",
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var tokenResult = _tokenHandler.WriteToken(token);

            return StatusCode(200, new SuccessResponse
            {
                Code = 200,
                Data = new
                {
                    token = tokenResult,
                }
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(401, new ErrorResponse { Code = 401, Message = ex.ToString() });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new ErrorResponse { Code = 503, Message = ex.ToString() });
        }
    }

    [HttpPost("information")]
    public IActionResult Information ([FromBody] InformationRequest request)
    {
        try
        {
            var validationOptions = new TokenValidationParameters
            {
                ValidIssuer = "localhost",
                IssuerSigningKey = _keyMaterial.GetSigningCredentials().Key,
                RequireExpirationTime = true,
                ValidAudience = "localhost",
            };

            SecurityToken token;
            var validationResult = _tokenHandler.ValidateToken(request.Token, validationOptions, out token);

            return StatusCode(200, new SuccessResponse
            {
                Code = 200,
                Data = new InformationResponse
                {
                    Active = DateTime.UtcNow >= token.ValidTo ? false : true,
                    Username = validationResult.FindFirstValue(ClaimTypes.Name),
                    Expires = token.ValidTo,
                    Roles = this.GetRoles(validationResult.FindAll(ClaimTypes.Role)),
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new ErrorResponse { Code = 503, Message = ex.ToString() });
        }
    }
}