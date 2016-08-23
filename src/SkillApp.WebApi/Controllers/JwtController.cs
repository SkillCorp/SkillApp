using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

using SkillApp.Entities.DTO.Login;
using SkillApp.WebApi.Options;
using SkillApp.Data.Entities;

namespace SkillApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private readonly JwtIssuerOptions _issuerOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtController(
            IOptions<JwtIssuerOptions> issuerOptions,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _issuerOptions = issuerOptions.Value;

            ThrowIfInvalidOptions();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]LoginUser user)
        {
            var identity = await GetClaimsIdentity(user);

            if (identity == null)
                return BadRequest("Invalid credentials");

            return new OkObjectResult(new LoginResponse
            {
                access_token = Encode(GetToken(identity.Claims)),
                expires_in = (int)_issuerOptions.ValidFor.TotalSeconds
            });
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Route("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody]LoginUser user)
        {
            var identity = new ApplicationUser { UserName = user.UserName };

            var result = await _userManager.CreateAsync(identity, user.Password);
            if (result.Succeeded)
            {
                var claims = await _userManager.AddClaimsAsync(identity, new[] { new Claim("AdminUser", "AdminUser") });
                if (claims.Succeeded)
                    return new OkResult();
            }
            return new BadRequestResult();
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [Route("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody]LoginUser user)
        {
            var identity = new ApplicationUser { UserName = user.UserName };

            var result = await _userManager.CreateAsync(identity, user.Password);
            if (result.Succeeded)
            {
                return new OkResult();
            }
            return new BadRequestResult();
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                            issuer: _issuerOptions.Issuer,
                            audience: _issuerOptions.Audience,
                            notBefore: _issuerOptions.NotBefore,
                            expires: _issuerOptions.Expiration,
                            signingCredentials: _issuerOptions.SigningCredentials,
                            claims: claims);
        }
        private static string Encode(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void ThrowIfInvalidOptions()
        {
            if (_issuerOptions == null)
                throw new ArgumentNullException(nameof(_issuerOptions));

            if (_issuerOptions.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (_issuerOptions.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (_issuerOptions.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<ClaimsIdentity> GetClaimsIdentity(LoginUser user)
        {
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            var identityClaims = new List<Claim>(await _userManager.GetClaimsAsync(identityUser));
            var canLogin = await _userManager.CheckPasswordAsync(identityUser, user.Password);

            var defaultClaims = await GetDefaultClaims(user);

            var claims = defaultClaims.Concat(identityClaims).ToList();

            if (canLogin)
                return new ClaimsIdentity(
                        new GenericIdentity(user.UserName, "Token"), claims);
            else
                return null;
        }

        private async Task<List<Claim>> GetDefaultClaims(LoginUser user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _issuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_issuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            };
        }
    }
}
