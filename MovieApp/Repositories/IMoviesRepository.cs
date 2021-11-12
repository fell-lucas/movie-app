using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Repositories
{
    public interface IMoviesRepository
    {
        Task<Movie> GetMovieAsync(string imdbId);
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<IEnumerable<Movie>> GetWatchedMoviesAsync();
        Task<IEnumerable<Movie>> GetUnwatchedMoviesAsync();
        Task CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(string imdbId);
    }
}