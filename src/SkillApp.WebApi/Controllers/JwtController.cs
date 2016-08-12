using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using SkillApp.Entities.DTO.Login;
using SkillApp.WebApi.Options;

namespace SkillApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class JwtController : Controller
    {
        private readonly JwtIssuerOptions _issuerOptions;

        public JwtController(IOptions<JwtIssuerOptions> issuerOptions)
        {
            _issuerOptions = issuerOptions.Value;
            ThrowIfInvalidOptions();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] LoginUser user)
        {
            var identity = await GetClaimsIdentity(user);
            if (identity == null)
                return BadRequest("Invalid credentials");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _issuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_issuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("AdminUser")
            };

            var token = GetToken(claims);
            var encodedToken = GetEncodedToken(token);
            return new OkObjectResult(new LoginResponse
            {
                access_token = encodedToken,
                expires_in = (int)_issuerOptions.ValidFor.TotalSeconds
            });
        }

        private JwtSecurityToken GetToken(Claim[] claims)
        {
            return new JwtSecurityToken(
                            issuer: _issuerOptions.Issuer,
                            audience: _issuerOptions.Audience,
                            notBefore: _issuerOptions.NotBefore,
                            expires: _issuerOptions.Expiration,
                            signingCredentials: _issuerOptions.SigningCredentials,
                            claims: claims);
        }
        private static string GetEncodedToken(JwtSecurityToken token)
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

        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static Task<ClaimsIdentity> GetClaimsIdentity(LoginUser user)
        {
            if (user.UserName == "Igor")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new[]
                  {
                    new Claim("AdminUser", "AdminUser")
                  }));
            }

            if (user.UserName == "NotIgor")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new Claim[] { }));
            }

            return NonExistingAccountOrInvalidCredentials();
        }
        private static Task<ClaimsIdentity> NonExistingAccountOrInvalidCredentials()
        {
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
