using K.Logger;
using Dividendos.API.Model.Messages;
using Dividendos.API.Model.Request.Auth;
using Dividendos.API.Model.Response.Common;
using Dividendos.CrossCutting.Identity;
using Dividendos.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using K.SocialLogin.Model;
using K.SocialLogin;
using Dividendos.API.Model.Request;
using Dividendos.Application.Interface;

namespace Dividendos.API.Controllers.v1
{
    /// <summary>
    /// Authentication Token
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly string _grantTypeRefreshToken = "refresh_token";
        private readonly string _grantTypePasswordToken = "password";
        private IConfiguration _configuration;
        //private readonly string _tokenProvider = "RefreshTokenProvider";
        private IDistributedCache _cache;
        private readonly ISocialLoginApp _socialLoginApp;
        private readonly IUserApp _userApp;
        private readonly ILogger _logger;

        /// <summary>
        /// AuthController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="tokenConfigurations"></param>
        /// <param name="signingConfigurations"></param>
        /// <param name="cache"></param>
        /// <param name="iConfiguration"></param>
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenConfigurations tokenConfigurations,
            IDistributedCache cache,
            IConfiguration iConfiguration,
            ISocialLoginApp socialLoginApp,
            IUserApp userApp,
            ILogger logger)
        {
            _userApp = userApp;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _cache = cache;
            _configuration = iConfiguration;
            _socialLoginApp = socialLoginApp;
            _logger = logger;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            ResultResponseObject<TokenResponse> resultService = new ResultResponseObject<TokenResponse>();
            ApplicationUser appUser = null;
            TokenResponse tokenResponse = null;

            if (loginModel.GrantType == _grantTypePasswordToken)
            {
                appUser =  HandleUserAuthentication(loginModel, resultService);
            }
            else if (loginModel.GrantType == _grantTypeRefreshToken)
            {
                appUser =  HandleRefreshToken(loginModel, resultService);
            }

            if (resultService.Success)
            {
                tokenResponse = GenerateJwtToken(appUser);

                resultService.Value = tokenResponse;
            }

            return Response(resultService);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("loginSocial/{provider}")]
        public async Task<IActionResult> LoginSocial(SocialProviderEnum provider, string token)
        {
            ResultResponseObject<TokenResponse> resultService = new ResultResponseObject<TokenResponse>();
            ApplicationUser appUser = null;
            TokenResponse tokenResponse = null;

            UserSocialMedia userSocialMedia = await _socialLoginApp.GetUserDataSocialMedia(provider, token);

            if (userSocialMedia != null)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(userSocialMedia.Email);
                if (user == null)
                {
                    UserRegisterVM userRegister = new UserRegisterVM();
                    userRegister.Password = Guid.NewGuid().ToString();
                    userRegister.Name = userSocialMedia.Name;
                    userRegister.Email = userSocialMedia.Email;
                    ResultResponseStringModel resultResponseStringModel = _userApp.RegisterNewUser(userRegister);

                    appUser = _userApp.GetUserByMail(userSocialMedia.Email).Value.FirstOrDefault();
                    tokenResponse = GenerateJwtToken(appUser);
                    resultService.Success = true;
                    resultService.Value = tokenResponse;
                }
                else
                {
                    appUser = _userManager.Users.SingleOrDefault(r => r.Email == user.Email);
                    tokenResponse = GenerateJwtToken(appUser);
                    resultService.Success = true;
                    resultService.Value = tokenResponse;
                }
            }
            else
            {
                resultService.ErrorMessages.Add(AuthMessage.InvalidLogin);
            }

            return Response(resultService);
        }

        #region [ Private Methods ]
        private ApplicationUser HandleUserAuthentication(LoginModel loginModel, ResultResponseBase resultResponseModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = new Microsoft.AspNetCore.Identity.SignInResult();
            ApplicationUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == loginModel.UserName && (r.Excluded == null || r.Excluded == false));

            if (appUser != null)
            {
                bool isBlocked =  IsUserBloked(appUser);

                if (!isBlocked)
                {
                    result =  _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, true).Result;
                    isBlocked =  IsUserBloked(appUser);
                }

                if (result.Succeeded)
                {
                     _userManager.ResetAccessFailedCountAsync(appUser);
                }

                if (!result.Succeeded)
                {
                    if (isBlocked)
                    {
                        _logger.SendDebugAsync(new { LoginFail = string.Concat(loginModel.UserName, " - Login Blocked") });
                        resultResponseModel.ErrorMessages.Add(AuthMessage.LoginBlocked);
                    }

                    if (!result.Succeeded && !result.IsLockedOut && appUser.AccessFailedCount.Equals(1))
                    {
                        _logger.SendDebugAsync(new { LoginFail = string.Concat(loginModel.UserName, " - Login fail InvalidPassword (attempt 1)") });
                        resultResponseModel.ErrorMessages.Add(AuthMessage.InvalidPassword);
                    }

                    if (!result.Succeeded && !result.IsLockedOut && appUser.AccessFailedCount.Equals(2))
                    {
                        _logger.SendDebugAsync(new { LoginFail = string.Concat(loginModel.UserName, " - Login fail InvalidPassword (attempt 2)") });
                        resultResponseModel.ErrorMessages.Add(AuthMessage.LoginSecondAttempts);
                    }

                    appUser = null;
                }
                else
                {
                    resultResponseModel.Success = true;
                }
            }
            else {
                resultResponseModel.ErrorMessages.Add(AuthMessage.InvalidLogin);
            }

            return appUser;
        }

        private ApplicationUser HandleRefreshToken(LoginModel loginModel, ResultResponseBase resultResponseModel)
        {
            bool credenciaisValidas = false;

            if (String.IsNullOrEmpty(loginModel.RefreshToken))
            {
                RefreshTokenData refreshTokenBase = null;

                string strTokenArmazenado = _cache.GetString(loginModel.RefreshToken);
                if (!String.IsNullOrWhiteSpace(strTokenArmazenado))
                {
                    refreshTokenBase = JsonConvert
                        .DeserializeObject<RefreshTokenData>(strTokenArmazenado);
                }

                credenciaisValidas = (refreshTokenBase != null &&
                    loginModel.RefreshToken == refreshTokenBase.RefreshToken);

                // Elimina o token de refresh já que um novo será gerado
                if (credenciaisValidas)
                    _cache.Remove(loginModel.RefreshToken);
            }

            JwtSecurityToken token = null;
            if (!string.IsNullOrWhiteSpace(loginModel.RefreshToken))
            {
                token = new JwtSecurityTokenHandler().ReadJwtToken(loginModel.RefreshToken);
            }

            if (token == null)
            {
                resultResponseModel.ErrorMessages.Add(AuthMessage.InvalidRefreshToken);
            }
            else if (token.ValidTo < DateTime.Now)
            {
                resultResponseModel.ErrorMessages.Add(AuthMessage.ExpiredRefreshToken);
            }
            else
            {
                string userId = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.UniqueName).Value;

                ApplicationUser user =  _userManager.FindByIdAsync(userId).Result;
                return user;
            }

            return null;
        }

        private TokenResponse GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            IList<Claim> userClaims =  _userManager.GetClaimsAsync(user).Result;
            IList<string> roles =  _userManager.GetRolesAsync(user).Result;
            var role = string.Empty;

            if (roles.Count() > 0)
            {
                role = roles.FirstOrDefault();
            }
            else
            {
                role = "User";
            }

            claims.AddRange(userClaims);

            var createdDate = DateTime.Now;
            var expiresTokenDate = createdDate.AddHours(Convert.ToDouble(_tokenConfigurations.HoursToExpireToken));            
            var expiresRefreshTokenDate = createdDate.AddHours(Convert.ToDouble(_tokenConfigurations.HoursToExpireRefreshToken));

            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Id, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("Email", user.Email)
                    }
                );

            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfigurations.Key));
            var creds = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = creds,
                Expires = expiresTokenDate,
                NotBefore = createdDate,
                IssuedAt = createdDate,
                Subject = identity
            });

            var securityRefreshToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = creds,
                Expires = expiresRefreshTokenDate,
                NotBefore = createdDate,
                IssuedAt = createdDate,
                Subject = identity
            });

            var token = handler.WriteToken(securityToken);
            var refreshToken = handler.WriteToken(securityRefreshToken);

            return new TokenResponse
            {
                Username = user.UserName,
                Token = token,
                RefreshToken = refreshToken,
                Roles = roles,
                Claims = claims,
                Expires = expiresTokenDate
            };
        }


        private bool IsUserBloked(ApplicationUser appUser)
        {
            // accessFailureCount Parameter
            int AccessFailureCount = 0;
            int.TryParse(_configuration.GetValue<string>("Parameters:AccessFailureCount"), out AccessFailureCount);

            bool isLockeOut =  _userManager.IsLockedOutAsync(appUser).Result;

            if (appUser.AccessFailedCount >= AccessFailureCount || isLockeOut)
            {
                 _userManager.SetLockoutEnabledAsync(appUser, true);
                if (_userManager.SupportsUserLockout &&  _userManager.GetLockoutEnabledAsync(appUser).Result)
                {
                    if (!isLockeOut) {
                         _userManager.AccessFailedAsync(appUser);
                        isLockeOut =  _userManager.IsLockedOutAsync(appUser).Result;
                    }
                }
            }

            return isLockeOut;
        }

        #endregion

    }
}