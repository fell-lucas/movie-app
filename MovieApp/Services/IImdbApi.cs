using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Responses;
using Refit;

namespace MovieApp.Services
{
  public interface IImdbApi
  {
    [Get("/API/SearchMovie/k_ea78tn3s/{fts}")]
    Task<ImdbResponse> GetMovies(string fts);
  }
}