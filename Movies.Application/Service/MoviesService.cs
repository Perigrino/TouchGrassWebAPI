using Movies.Application.Interface;
using Movies.Application.Model;

namespace Movies.Application.Service;

public class MoviesService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MoviesService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        return await _movieRepository.CreateMovie(movie);
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await _movieRepository.GetMovieById(id);
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        return await _movieRepository.GetMovieBySlug(slug);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await _movieRepository.GetMoviesAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        var movieExists = await _movieRepository.MovieExist(movie.Id);
        if (!movieExists)
        {
            return null;
        }
        await _movieRepository.UpdateMovie(movie);
        return movie;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _movieRepository.DeleteMovie(id);
    }
}