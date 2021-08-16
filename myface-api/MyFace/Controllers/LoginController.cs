using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;

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
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var authenticated = _posts.IsAthenticated(authHeader);

            return new AuthenticationResult()
            {
                Success = authenticated
            };
        }
    }
}