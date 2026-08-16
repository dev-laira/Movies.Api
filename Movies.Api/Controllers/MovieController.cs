using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using MovieEndpoint = Movies.Api.ApiEndpoint.Movies;

namespace Movies.Api.Controllers
{
    
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService; 
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(MovieEndpoint.Create)]
        public async Task<IActionResult> Create([FromBody]CreateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie();
            await _movieService.CreateAsync(movie,token:token);

            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Slug }, movie.MapToResponse());
        }

        [AllowAnonymous]
        [HttpGet(MovieEndpoint.Get)]
        public async Task<IActionResult> Get([FromRoute]string idOrSlug, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movie = Guid.TryParse(idOrSlug, out Guid id) ? 
                await _movieService.GetByIdAsync(id, userId:userId,  token: token) :
                await _movieService.GetBySlugAsync(idOrSlug, userId: userId, token: token);
            if(movie is null)
            {
                return NotFound();
            }

            var response = movie.MapToResponse();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(MovieEndpoint.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movies = await _movieService.GetAllAsync(userId:userId, token: token);

            var moviesResponse = movies.MapToResponse();
            return Ok(moviesResponse);
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPut(MovieEndpoint.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movie = request.MapToMovie(id);
            var updated = await _movieService.UpdateByIdAsync(request.MapToMovie(id), userId:userId, token: token);

            if(updated is null)
            {
                return NotFound();
            }

            return Ok(movie.MapToResponse());
        }

        [Authorize(AuthConstants.AdminPolicyName)]
        [HttpDelete(MovieEndpoint.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _movieService.DeleteByIdAsync(id, token: token);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
