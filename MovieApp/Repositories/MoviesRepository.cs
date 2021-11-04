using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MovieApp.Entities;
using MovieApp.Services;

namespace MovieApp.Repositories
{
  public class MoviesRepository : IMoviesRepository
  {
    private const string databaseName = "movieApp";
    private const string collectionName = "movies";
    private readonly IMongoCollection<Movie> itemsCollection;
    private readonly IImdbApi imdbApi;

    public MoviesRepository(IMongoClient mongoClient, IImdbApi imdbApi)
    {
      IMongoDatabase database = mongoClient.GetDatabase(databaseName);
      itemsCollection = database.GetCollection<Movie>(collectionName);
      this.imdbApi = imdbApi;
    }

    public async Task CreateMovieAsync(Movie movie)
    {
      await itemsCollection.InsertOneAsync(movie);
    }

    public Task DeleteMovieAsync(Movie movie)
    {
      throw new System.NotImplementedException();
    }

    public Task<Movie> GetMovieAsync(string imdbId)
    {
      throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Movie>> GetMoviesAsync()
    {
      throw new System.NotImplementedException();
    }

    public async Task<IEnumerable<Movie>> SearchMoviesAsync(string fts)
    {
      var response = await imdbApi.SearchMovies(fts);
      var movies = response.results.Select(movieResponse => movieResponse.AsMovie());
      return movies;
    }

    public Task UpdateMovieAsync(Movie movie)
    {
      throw new System.NotImplementedException();
    }
  }
}