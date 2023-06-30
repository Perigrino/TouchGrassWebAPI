namespace Movie.Contracts.Requests;

public class UpdateMovieRequest
{
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
    public required int Rating { get; set; }
    public required int? UserRating { get; set; }
    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
}