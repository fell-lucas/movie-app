using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Dtos.Movie.Responses
{
    [ExcludeFromCodeCoverage]
    public record MovieListResponse
    {
        public string searchType { get; init; }
        public string expression { get; init; }
        public IEnumerable<MovieResponse> results { get; init; }
        public string errorMessage { get; init; }
    }

    public record MovieResponse
    {
        public string id { get; init; }
        public string image { get; init; }
        public string title { get; init; }
        public string description { get; init; }
    }
}