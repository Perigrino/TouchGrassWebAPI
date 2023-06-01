using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Interface;
using Movies.Application.Repository;
using Movies.Application.Service;

namespace Movies.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddSingleton<IMovieRepository, MoviesRepository>();
        service.AddSingleton<IMovieService, MoviesService>();
        //service.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return service;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
        
    }
}