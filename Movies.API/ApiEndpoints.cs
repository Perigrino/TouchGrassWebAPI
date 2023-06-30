namespace Movies.API;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";
        
        public const string Get = $"{Base}/{{idorSlug}}";
        public const string GetAll = Base;
        public const string Create = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        
        public const string Rate = $"{Base}/{{id:guid}}/ratings";
        public const string DeleteRating = $"{Base}/{{id:guid}}/ratings";
    }
    
    public static class Ratings
    {
        public const string Base = $"{ApiBase}/ratings";
        public const string GetUserRatings = $"{ApiBase}/me";
    }
}