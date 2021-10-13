using Microsoft.AspNetCore.Mvc;
using MyFace.Helpers;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    public class InteractionsController
    {
        [ApiController]
        [Route("/interactions")]
        public class UsersController : ControllerBase
        {
            private readonly IInteractionsRepo _interactions;
            private readonly IPostsRepo _posts;

            public UsersController(IInteractionsRepo interactions, IPostsRepo posts)
            {
                _interactions = interactions;
                _posts = posts;
            }
        
            [HttpGet("")]
            public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search)
            {
                var interactions = _interactions.Search(search);
                var interactionCount = _interactions.Count(search);
                return InteractionListResponse.Create(search, interactions, interactionCount);
            }

            [HttpGet("{id}")]
            public ActionResult<InteractionResponse> GetById([FromRoute] int id)
            {
                var interaction = _interactions.GetById(id);
                return new InteractionResponse(interaction);
            }

            [HttpPost("create")]
            public IActionResult Create([FromBody] CreateInteractionRequest newUser)
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                var token = HttpContext.Request.Cookies["JWT"];
                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}/";
                var authenticated = JWT.ValidateCurrentToken(token, baseUrl);

                if (!authenticated)
                {
                    return Unauthorized();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            
                var interaction = _interactions.Create(newUser, authHeader);

                var url = Url.Action("GetById", new { id = interaction.Id });
                var responseViewModel = new InteractionResponse(interaction);
                return Created(url, responseViewModel);
            }

            [HttpDelete("{id}")]
            public IActionResult Delete([FromRoute] int id)
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var authenticated = _posts.IsAthenticated(authHeader);
                var admin = _posts.IsAdmin(authHeader);

                if (!authenticated)
                {
                    return Unauthorized();
                }

                if (!admin)
                {
                    return StatusCode(403);
                }

                _interactions.Delete(id);
                return Ok();
            }
        }
    }
}