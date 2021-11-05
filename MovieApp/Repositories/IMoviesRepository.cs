using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Repositories
{
  public interface IMoviesRepository
  {
    Task<IEnumerable<Movie>> SearchMoviesFromApiAsync(string fts);
    Task<Movie> SearchSingleMovieFromApiAsync(string imdbId);
    Task<Movie> GetMovieFromDbAsync(string imdbId);
    Task<IEnumerable<Movie>> GetAllMoviesFromDbAsync();
    Task<IEnumerable<Movie>> GetWatchedMoviesFromDbAsync();
    Task<IEnumerable<Movie>> GetUnwatchedMoviesFromDbAsync();
    Task CreateMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(string imdbId);

  }
}