using JWT.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidateTokenController : ControllerBase
    {

        public ValidateTokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost]
        public IActionResult ValidateToken([FromBody] JsonDto json)
        {

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true, // Validate the server (ValidateIssuer = true) that generates the token.
                ValidateAudience = true, // Validate the recipient of the token is authorized to receive (ValidateAudience = true)
                ValidateLifetime = true, // Check if the token is not expired and the signing key of the issuer is valid (ValidateLifetime = true)
                ValidateIssuerSigningKey = true, // Validate signature of the token (ValidateIssuerSigningKey = true)
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(json.token, validationParams, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return Ok(jwtToken);
            }
            catch
            {

                return Unauthorized();
            }
        }
    }
}
