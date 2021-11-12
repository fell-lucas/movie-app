using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Dtos.Movie.Responses;
using MovieApp.Entities;

namespace MovieApp.Services
{
    public interface IImdbSearchService
    {
        Task<IEnumerable<Movie>> SearchMoviesFromApiAsync(string fts);
        Task<Movie> SearchSingleMovieFromApiAsync(string imdbId);
        Task<DetailedMovieResponse> SearchDetailedMovieFromApiAsync(string imdbId);

    }
}