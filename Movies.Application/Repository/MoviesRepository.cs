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

    public async Task<IEnumerable<Movie>> GetMoviesAsync(CancellationToken token = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition("""
            select m.*, string_agg(g.name, ',') as genres 
            from movies m left join genres g on m.id = g.movieid
            group by id 
            """, cancellationToken: token));
        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<Movie?> GetMovieById(Guid movieId, CancellationToken token = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select * from movies where movieId = @id
            """, new { movieId }, cancellationToken: token));
        
        if (movie is null)
        {
            return null;
        }
        
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from genres where movieid = @id 
            """, new { movieId }, cancellationToken: token));
        
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        return movie;
    }

    public async Task<Movie?> GetMovieBySlug(string slug, CancellationToken token = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select * from movies where slug = @slug
            """, new { slug }, cancellationToken: token));
        
        if (movie is null)
        {
            return null;
        }
        
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            select name from genres where movieid = @id 
            """, new { slug }, cancellationToken: token));
        
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        return movie;
    }

    public async Task<bool> CreateMovie(Movie movie,CancellationToken token = default)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
            insert into movies (id, slug, title, yearofrelease)
            values (@Id, @Slug, @Title,@YearOfRelease)
            """, movie, cancellationToken: token));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition("""
                    insert into genres (movieid, name)
                    values (@Movieid, @Name)
                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
            }
        }
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> UpdateMovie(Movie movie, CancellationToken token = default)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
            delete from genres where movieid = @id
            """, new { id = movie.Id }, cancellationToken: token));
        
        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                    insert into genres (movieId, name) 
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
        }
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
            where id = @Id
            """, movie, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteMovie(Guid id, CancellationToken token = default)
    {
        var connection = await _connectionFactory.CreateConnectionAsync();
        var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(new CommandDefinition("""
        delete from genre where movieid = @Id
        """, new {id}, cancellationToken: token));
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
        delete from movies where id = @Id
        """, new {id}, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> MovieExist(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from movies where id = @id
            """, new { id }));
    }
}