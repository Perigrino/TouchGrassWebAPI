using Dapper;
using Movies.Application.Database;
using Movies.Application.Interface;
using Movies.Application.Model;

namespace Movies.Application.Repository;

public class MoviesRepository: IMovieRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MoviesRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Movie?> GetMovieById(Guid movieId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select * from movies where movieId = @id
            """, new { movieId }));
        
        if (movie is null)
        {
            return null;
        }
        
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from genres where movieid = @id 
            """, new { movieId }));
        
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        return movie;
    }

    public async Task<Movie?> GetMovieBySlug(string slug)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateMovie(Movie movie)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
            insert into movies (id, slug, title, yearofrelease)
            values (@Id, @Slug, @Title,@YearOfRelease)
            """, movie));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition("""
                    insert into genre (movieId, name)
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }));
            }
        }
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> UpdateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteMovie(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> MovieExist(Guid id)
    {
        throw new NotImplementedException();
    }
}