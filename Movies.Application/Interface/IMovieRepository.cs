using Movies.Application.Model;

namespace Movies.Application.Interface;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<Movie?> GetMovieById(Guid movieId);
    Task<Movie?> GetMovieBySlug(string slug);
    Task<bool> CreateMovie(Movie movie);
    Task<bool> UpdateMovie(Movie movie);
    Task<bool> DeleteMovie(Guid id);
    Task<bool> MovieExist(Guid id);
}