using BL.Model.User;
using BL.Services;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpendingTracker.Config;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Auth.Response;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly GoogleSettings _googleSettings;
        private readonly JWTSettings _jwtSettings;

        public AuthService(
            IUserService userService,
            IOptions<GoogleSettings> googleSettings,
            IOptions<JWTSettings> jwtSettings)
        {
            _userService = userService;
            _googleSettings = googleSettings.Value;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> GoogleSignIn(string googleIdToken)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(
                    googleIdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new string[] { _googleSettings.ClientId }
                    });

                int existingUserId = await _userService.GetUserIdByGoogleId(payload.Subject);

                var user = new UserDomain
                {
                    Id = existingUserId,
                    Email = payload.Email,
                    GoogleId = payload.Subject,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    PictureUrl = payload.Picture
                };

                user = await _userService.UpsertUser(user);
                
                return new AuthResponse
                {
                    AccessToken = CreateToken(user),
                    User = user.ToResponse()
                };
            }
            catch (InvalidJwtException)
            {
                return null;
            }
        }

        private string CreateToken(UserDomain user, bool isRefreshToken = false)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(isRefreshToken ? _jwtSettings.RefreshTokenSecret : _jwtSettings.AccessTokenSecret));
            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }),
                Expires = DateTime.Now.AddHours(24)
            };

            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
