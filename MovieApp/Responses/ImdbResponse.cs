using System.Collections.Generic;

namespace MovieApp.Responses
{
  public record ImdbResponse
  {
    public string searchType { get; init; }
    public string expression { get; init; }
    public IEnumerable<MovieResponse> results { get; init; }
    public string errorMessage { get; init; }
  }
}