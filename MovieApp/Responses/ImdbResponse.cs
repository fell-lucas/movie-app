using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Responses
{
  [ExcludeFromCodeCoverage]
  public record ImdbResponse
  {
    public string searchType { get; init; }
    public string expression { get; init; }
    public IEnumerable<MovieResponse> results { get; init; }
    public string errorMessage { get; init; }
  }
}