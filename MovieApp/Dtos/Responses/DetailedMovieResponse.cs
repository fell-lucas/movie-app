namespace MovieApp.Dtos.Movie.Responses
{
    public record DetailedMovieResponse
    {
        public string id { get; init; }
        public string title { get; init; }
        public string year { get; init; }
        public string image { get; init; }
        public string releaseDate { get; init; }
        public string runtimeStr { get; init; }
        public string plot { get; init; }
        public string awards { get; init; }
        public DirectorResponse[] directorList { get; init; }
        public ActorResponse[] actorList { get; init; }
        public string genres { get; init; }
        public string languages { get; init; }
        public string imDbRating { get; init; }
        public string metacriticRating { get; init; }
        public string[] keywordList { get; init; }
    }

    public record DirectorResponse
    {
        public string id { get; init; }
        public string name { get; init; }
    }

    public record ActorResponse
    {
        public string id { get; init; }
        public string name { get; init; }
        public string image { get; init; }
        public string asCharacter { get; init; }
    }
}