using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyFace.Helpers;
using MyFace.Repositories;
using System;

namespace MyFace.Controllers
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Role { get; set; }
    }

    [ApiController]
    [Route("/login")]
    public class LoginController : Controller
    {
        private readonly IPostsRepo _posts;

        public LoginController(IPostsRepo postsRepo)
        {
            _posts = postsRepo;
        }

        [HttpGet("")]
        public IActionResult IsValidAuthentication()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var authenticated = _posts.IsAthenticated(authHeader);
            var user = _posts.GetUserFromEncoded(authHeader);
            var token = JWT.GenerateToken(user.Id, user.Role, baseUrl);

            if (authenticated)
            {
                var options = new CookieOptions();
                options.Expires = DateTime.Now.AddMinutes(1);
                options.HttpOnly = true;
                Response.Cookies.Append("JWT", token, options);

                return Ok(new AuthenticationResult()
                {
                    Success = authenticated,
                    Role = user.Role.ToString()
                });
            }

            return Unauthorized();
        }

        [HttpGet("validate")]
        public IActionResult Validate()
        {
            var token = HttpContext.Request.Cookies["JWT"];
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var valid = JWT.ValidateCurrentToken(token, baseUrl);
            var claim = JWT.GetClaim(token, "Role");

            if (valid)
            {
                return Ok(new AuthenticationResult()
                {
                    Success = valid,
                    Role = claim
                });
            }

            return Forbid();
        }
    }
}