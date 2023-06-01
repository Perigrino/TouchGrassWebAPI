namespace Movies.API;

public static class APIEndpoints
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
    }
}