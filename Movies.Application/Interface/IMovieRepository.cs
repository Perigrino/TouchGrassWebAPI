using Movies.Application.Model;

namespace Movies.Application.Interface;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetMoviesAsync(CancellationToken token = default);
    Task<Movie?> GetMovieById(Guid movieId, CancellationToken token = default);
    Task<Movie?> GetMovieBySlug(string slug, CancellationToken token = default);
    Task<bool> CreateMovie(Movie movie, CancellationToken token = default);
    Task<bool> UpdateMovie(Movie movie, CancellationToken token = default);
    Task<bool> DeleteMovie(Guid id, CancellationToken token = default);
    Task<bool> MovieExist(Guid id);
}