using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Responses;
using Refit;

namespace MovieApp.Services
{
  public interface IImdbApi
  {
    [Get("/API/SearchMovie/{apikey}/{fts}")]
    Task<ImdbResponse> SearchMovies(string fts, string apikey = Constants.apiKey);

    [Get("/API/SearchMovie/{apikey}/{imdbId}")]
    Task<ImdbResponse> SearchSingleMovie(string imdbId, string apikey = Constants.apiKey);
  }
}