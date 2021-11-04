using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Entities;
using MovieApp.Enums;

namespace MovieApp.Repositories
{
  public interface IMoviesRepository
  {
    Task<IEnumerable<Movie>> SearchMoviesFromApiAsync(string fts);
    Task<Movie> SearchSingleMovieFromApiAsync(string imdbId);
    Task<Movie> GetMovieFromDbAsync(string imdbId);
    Task<IEnumerable<Movie>> GetMoviesFromDbAsync(WatchedFilter filter);
    Task CreateMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(Movie movie);

  }
}