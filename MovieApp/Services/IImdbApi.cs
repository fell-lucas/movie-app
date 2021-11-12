using System.Threading.Tasks;
using MovieApp.Dtos.Movie.Responses;
using Refit;

namespace MovieApp.Services
{
    public interface IImdbApi
    {
        [Get("/API/SearchMovie/{apikey}/{fts}")]
        Task<MovieListResponse> SearchMovies(string fts, string apikey = Constants.apiKey);

        [Get("/API/SearchMovie/{apikey}/{imdbId}")]
        Task<MovieListResponse> SearchSingleMovie(string imdbId, string apikey = Constants.apiKey);

        [Get("/API/Title/{apikey}/{imdbId}")]
        Task<DetailedMovieResponse> SearchDetailedMovie(string imdbId, string apikey = Constants.apiKey);
    }
}