using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [Route("feed")]
    public class FeedController : Controller
    {
        private readonly IPostsRepo _posts;

        public FeedController(IPostsRepo posts)
        {
            _posts = posts;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest)
        {
            var token = HttpContext.Request.Cookies["JWT"];
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
            var valid = JWT.ValidateCurrentToken(token, baseUrl);

            if (!valid)
            {
                return Unauthorized();
            }

            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}