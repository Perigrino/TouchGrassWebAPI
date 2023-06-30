using Movie.Contracts.Requests;
using Movie.Contracts.Response;
using Movies.Application.Model;

namespace Movies.API.Mapping;

public static class ContractMapping
{
    public static Application.Model.Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Application.Model.Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Rating = request.Rating,
            UserRating = request.UserRating,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };
    }
    
    public static MovieResponse MapsToResponse(this Application.Model.Movie movie)
    {
        return new MovieResponse()
        {
            Id = movie.Id,
            Title = movie.Title,
            Rating = movie.Rating,
            UserRating = movie.UserRating,
            Slug = movie.Slug,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres.ToList()

        };
    }
    
    public static MoviesResponse MapsToResponse(this IEnumerable<Application.Model.Movie> movies)
    {
        return new MoviesResponse()
        {
            Items = movies.Select(MapsToResponse)
        };
    }
    
    public static Application.Model.Movie MapToMovie(this UpdateMovieRequest request, Guid Id)
    {
        return new Application.Model.Movie
        {
            Id = Id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Rating = request.Rating,
            UserRating = request.UserRating,
            Genres = request.Genres.ToList()
        };
    }
    
    public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
    {
        return ratings.Select(x => new MovieRatingResponse
        {
            Rating = x.Rating,
            Slug = x.Slug,
            MovieId = x.MovieId
        });
    }
}