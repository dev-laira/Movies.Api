using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize]
        [HttpPut(ApiEndpoint.Movies.Rate)]
        public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var rate = await _ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);

            if (!rate)
            {
                return NotFound();
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete(ApiEndpoint.Movies.DeleteRating)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var deleteRating = await _ratingService.DeleteRatingAsync(id, userId!.Value, token); 

            if(!deleteRating) { return NotFound(); }

            return Ok();
        }

        [Authorize]
        [HttpGet(ApiEndpoint.Ratings.GetUserRatings)]
        public async Task<IActionResult> GetUserRatings(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var ratings = await _ratingService.GetUserRatingsAsync(userId!.Value, token);

            return Ok(ratings.MapToResponse());
        }
    }
}
