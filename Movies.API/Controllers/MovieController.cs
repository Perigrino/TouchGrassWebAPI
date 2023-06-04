using Microsoft.AspNetCore.Mvc;
using Movie.Contracts.Requests;
using Movies.API.Mapping;
using Movies.Application.Service;

namespace Movies.API.Controllers;

[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/Movie
    [HttpGet(APIEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetMovies(CancellationToken token)
    {
        var movies = await _movieService.GetAllAsync();
        return Ok(movies.MapsToResponse());
    }

    // GET: api/Movie/5
    [HttpGet(APIEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id)
            : await _movieService.GetBySlugAsync(idOrSlug);
        if (movie == null)
            return NotFound();

        return Ok(movie.MapsToResponse());
    }

    // POST: api/Movie
    [HttpPost(APIEndpoints.Movies.Create)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request, CancellationToken token)
    {
        var movie = request.MapToMovie();
        await _movieService.CreateAsync(movie);
        var movieResponse = movie.MapsToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movieResponse);
    }

    // PUT: api/Movie/5
    [HttpPut(APIEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie(Id);
        var updated = await _movieService.UpdateAsync(movie);
        if (updated is null)
        {
            return NotFound();
        }
        var response = movie.MapsToResponse();
        return Ok(response);

    }

    // DELETE: api/Movie/5
    [HttpDelete(APIEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        var movie = await _movieService.DeleteByIdAsync(id);
        if (!movie)
            return NotFound();
        return Ok("Movie has been deleted successfully");
    }
}