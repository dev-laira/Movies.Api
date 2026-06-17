using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using MovieEndpoint = Movies.Api.ApiEndpoint.Movies;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository; 
        }

        [HttpPost(MovieEndpoint.Create)]
        public async Task<IActionResult> Create([FromBody]CreateMovieRequest request)
        {
            var movie = request.MapToMovie();
            await _movieRepository.CreateAsync(movie);

            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Slug }, movie.MapToResponse());
        }

        [HttpGet(MovieEndpoint.Get)]
        public async Task<IActionResult> Get([FromRoute]string idOrSlug)
        {
            var movie = Guid.TryParse(idOrSlug, out Guid id) ? 
                await _movieRepository.GetByIdAsync(id):
                await _movieRepository.GetBySlugAsync(idOrSlug);
            if(movie is null)
            {
                return NotFound();
            }

            var response = movie.MapToResponse();
            return Ok(response);
        }

        [HttpGet(MovieEndpoint.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieRepository.GetAllAsync();

            var moviesResponse = movies.MapToResponse();
            return Ok(moviesResponse);
        }

        [HttpPut(MovieEndpoint.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
        {

            var movie = request.MapToMovie(id);
            var updated = await _movieRepository.UpdateByIdAsync(request.MapToMovie(id));

            if(!updated)
            {
                return NotFound();
            }

            return Ok(movie.MapToResponse());
        }

        [HttpDelete(MovieEndpoint.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _movieRepository.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
