using Movies.Application.Model;

namespace Movies.Application.Service;

public class MoviesService : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}