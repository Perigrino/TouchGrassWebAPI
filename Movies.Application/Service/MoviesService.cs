using FluentValidation;
using Movies.Application.Interface;
using Movies.Application.Model;

namespace Movies.Application.Service;

public class MoviesService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _validator;

    public MoviesService(IMovieRepository movieRepository, IValidator<Movie> validator)
    {
        _movieRepository = movieRepository;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await _movieRepository.CreateMovie(movie, token);
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return await _movieRepository.GetMovieById(id, userId, token);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        return await _movieRepository.GetMovieBySlug(slug, userId, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
    {
        return await _movieRepository.GetMoviesAsync(userId, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        await _validator.ValidateAndThrowAsync (movie, cancellationToken: token);
        var movieExists = await _movieRepository.MovieExist(movie.Id, token);
        if (!movieExists)
            return null;
        await _movieRepository.UpdateMovie(movie, token);
        return movie;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _movieRepository.DeleteMovie(id, token);
    }
}