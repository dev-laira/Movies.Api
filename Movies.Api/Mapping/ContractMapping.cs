using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping
{
    public static class ContractMapping
    {
        public static Movie MapToMovie(this CreateMovieRequest request)
        {
            return new Movie { 
                Id = Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
        {
            return new Movie
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static MovieResponse MapToResponse(this Movie movie)
        {
            return new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                UserRating = movie.UserRating,
                Rating = movie.Rating,
                YearOfRelease = movie.YearOfRelease,
                Slug = movie.Slug,
                Genres = movie.Genres
            };
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
        {
            return new MoviesResponse { Items = movies.Select(MapToResponse) };
        }

        public static RatingResponse MapToResponse(this MovieRating rating)
        {
            return new RatingResponse { MovieId = rating.MovieId , Rating = rating.Rating};
        }

        public static RatingsResponse MapToResponse(this IEnumerable<MovieRating> ratings)
        {
            return new RatingsResponse { Items = ratings.Select(MapToResponse) };
        }
    }
}
