using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieApp.Entities;
using MovieApp.Enums;
using MovieApp.Services;

namespace MovieApp.Repositories
{
  public class MoviesRepository : IMoviesRepository
  {
    private const string databaseName = "movieApp";
    private const string collectionName = "movies";
    private readonly IMongoCollection<Movie> itemsCollection;
    private readonly IImdbApi imdbApi;
    private readonly FilterDefinitionBuilder<Movie> filterBuilder = Builders<Movie>.Filter;

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

    public async Task<Movie> GetMovieFromDbAsync(string imdbId)
    {
      var filter = filterBuilder.Eq(movie => movie.ImdbId, imdbId);
      return await itemsCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesFromDbAsync(WatchedFilter watchedFilter)
    {
      FilterDefinition<Movie> filter = new BsonDocument();
      switch (watchedFilter)
      {
        case WatchedFilter.Watched:
          filter = filterBuilder.Eq(movie => movie.Watched, true);
          break;
        case WatchedFilter.Unwatched:
          filter = filterBuilder.Eq(movie => movie.Watched, false);
          break;
      }
      return await itemsCollection.Find(filter).ToListAsync();
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

    public Task UpdateMovieAsync(Movie movie)
    {
      throw new System.NotImplementedException();
    }
  }
}