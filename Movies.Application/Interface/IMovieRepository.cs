using Movies.Application.Model;

namespace Movies.Application.Interface;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMoviesAsync(Guid? userId = default, CancellationToken token = default);
    Task<Movie?> GetMovieById(Guid movieId, Guid? userId = default, CancellationToken token = default);
    Task<Movie?> GetMovieBySlug(string slug, Guid? userId = default, CancellationToken token = default);
    Task<bool> CreateMovie(Movie movie, CancellationToken token = default);
    Task<bool> UpdateMovie(Movie movie, CancellationToken token = default);
    Task<bool> DeleteMovie(Guid id, CancellationToken token = default);
    Task<bool> MovieExist(Guid id, CancellationToken token = default);
}