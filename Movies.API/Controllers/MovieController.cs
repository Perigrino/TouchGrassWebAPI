using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Contracts.Requests;
using Movies.API.Auth;
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
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetMovies(Guid? userId, CancellationToken token)
    {
        var user = HttpContext.GetUserId();
        var movies = await _movieService.GetAllAsync(userId, token);
        return Ok(movies.MapsToResponse());
    }

    // GET: api/Movie/5
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var user = HttpContext.GetUserId();
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, user, token)
            : await _movieService.GetBySlugAsync(idOrSlug,user, token);
        if (movie == null)
            return NotFound();

        return Ok(movie.MapsToResponse());
    }

    // POST: api/Movie
    [Authorize (AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request, CancellationToken token)
    {
        var movie = request.MapToMovie();
        await _movieService.CreateAsync(movie, token);
        var movieResponse = movie.MapsToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movieResponse);
    }

    // PUT: api/Movie/5
    [Authorize (AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var user = HttpContext.GetUserId();
        var movie = request.MapToMovie(id);
        var updated = await _movieService.UpdateAsync(movie, token);
        if (updated is null)
        {
            return NotFound();
        }
        var response = movie.MapsToResponse();
        return Ok(response);

    }

    // DELETE: api/Movie/5
    [Authorize (AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        var movie = await _movieService.DeleteByIdAsync(id, token);
        if (!movie)
            return NotFound();
        return Ok("Movie has been deleted successfully");
    }
}