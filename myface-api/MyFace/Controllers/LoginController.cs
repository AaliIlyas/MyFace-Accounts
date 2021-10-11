using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Repositories;
using System;

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
            var baseUrl = Url.Content("~/");
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
    }
}