using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Dtos.Movie.Responses;
using MovieApp.Entities;

namespace MovieApp.Services
{
    public class ImdbSearchService : IImdbSearchService
    {
        private readonly IImdbApi imdbApi;
        public ImdbSearchService(IImdbApi imdbApi)
        {
            this.imdbApi = imdbApi;
        }

        public async Task<IEnumerable<Movie>> SearchMoviesFromApiAsync(string fts)
        {
            var response = await imdbApi.SearchMovies(fts);
            var movies = response.results.Select(movieResponse => movieResponse.AsMovie());
            return movies;
        }

        public async Task<Movie> SearchSingleMovieFromApiAsync(string imdbId)
        {
            var response = await imdbApi.SearchSingleMovie(imdbId);
            var movie = response.results.Single().AsMovie();
            return movie;
        }

        public async Task<DetailedMovieResponse> SearchDetailedMovieFromApiAsync(string imdbId)
        {
            return await imdbApi.SearchDetailedMovie(imdbId);
        }
    }
}