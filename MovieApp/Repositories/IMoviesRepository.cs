using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Repositories
{
  public interface IMoviesRepository
  {
    Task<IEnumerable<Movie>> SearchMoviesAsync(string fts);
    Task<Movie> GetMovieAsync(string imdbId);
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task CreateMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(Movie movie);

  }
}