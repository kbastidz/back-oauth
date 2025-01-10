using Core.service;
using db.dominio;
using db.Response;
//using edu.service;
using Microsoft.AspNetCore.Mvc;
using api_gestion_escolar.TokenGenerator;

namespace api_gestion_escolar.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class OTCController : ControllerBase
    {
        private readonly IAuthService _authService;
        //private readonly IEduService _eduService;

        private readonly TokenValidationFilter _config;

        public OTCController(IAuthService authService, TokenValidationFilter config = null)
        {
           //_eduService = eduService;
            _authService = authService;
            _config = config; 
        }

        #region Back-Login
        [HttpPost("generaToken")]
        public string GeneraToken(string user)
        {
            return _config.GenerateToken(user);
        }

        [HttpPost("refreshToken")]
        public string RefreshToken(string token)
        {
           return _config.RefreshToken(token);
        }

        [HttpPost("validateLogin")]
        public async Task<RsTrxService> ValidateLogin(Login request)
        {
           
            var result = await _authService.validateLogin(request);
            if ((int) result.Status == 200)
            {
                result.Token = _config.GenerateToken(request.User);
            }
            return result;
        }

        [HttpPost("rememberPassword")]
        public async Task<RsTrxService> RememberPassword(Login request)
        {
            return await _authService.rememberPassword(request);
        }

        [HttpPost("unlockUser")]
        public async Task<RsTrxService> UnlockUser(Login request)
        {
            return await _authService.unlockUser(request);
        }

        [HttpGet("consultPersonId/{id}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ConsultPersonId(int id)
        {
            var response = await _authService.consultPersonForId(id);
            return Ok(response);
        }

        [HttpPost("registerUser")]
        public async Task<RsTrxService> RegisterUser(Usuario request)
        {
            return await _authService.registerUser(request);
        }

        [HttpPut("updateUser")]
        public async Task<RsTrxService> UpdateUser(Usuario request)
        {
            return await _authService.updateUser(request);
        }
        #endregion


    }
}