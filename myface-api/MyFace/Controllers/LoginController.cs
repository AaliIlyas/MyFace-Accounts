using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFace.Helpers;
using MyFace.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace MyFace.Controllers
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
    }

    [ApiController]
    [Route("/login")]
    public class LoginController : Controller
    {
        private readonly IPostsRepo _posts;

        public LoginController (IPostsRepo postsRepo)
        {
            _posts = postsRepo;
        }

        [HttpGet("")]
        public AuthenticationResult IsValidAuthentication ()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var authenticated = _posts.IsAthenticated(authHeader);
            var user = _posts.GetUserFromEncoded(authHeader);

            if (authenticated)
            {
                var token = JWT.GenerateToken(user.Id, user.Role, baseUrl);
            }

            return new AuthenticationResult()
            {
                Success = authenticated
            };
        }

        [HttpGet("validate")]
        public bool ValidateCurrentToken(string token)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = baseUrl;
            var myAudience = "http://localhost:3000";

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        [HttpGet("claim")]
        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}