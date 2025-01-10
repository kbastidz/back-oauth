using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using db.Enum;
using db.Response;

namespace api_gestion_escolar.TokenGenerator
{
    public class TokenValidationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _config;
        private readonly List<string> _allowedEndpoints = new List<string>
        {
            "validateLogin",
            "generaToken"
        };
        public TokenValidationFilter(IConfiguration config)
        {
            _config = config;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"];

            if (_allowedEndpoints.Any(endpoint => endpoint.Equals(actionName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
            {
                var response = new RsTrxService
                {
                    Status = StatusCode.Unauthorized,
                    Code = 401,
                    Message = "Token inválido o no proporcionado.",
                    Token = String.Empty
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = 401
                };
            }
        }

        #region-Token
        public string GenerateToken(string username)
        {
            var secretKey = _config["Jwt:Key"];
            var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string RefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _config["Jwt:Key"];
            var key = Encoding.UTF8.GetBytes(secretKey!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

                return GenerateToken(username);
            }
            catch (Exception)
            {
                return "Token no válido o expirado";
            }
        }

        private bool ValidateToken(string token)
        {
            var secretKey = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validar el token
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourIssuer",
                    ValidAudience = "yourAudience",
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);

               
                return true;
            }
            catch (SecurityTokenExpiredException)
            {               
                return false;
            }
            catch (Exception)
            {               
                return false;
            }
        }

        #endregion

    }
}
