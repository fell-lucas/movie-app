using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieApp.Entities;
using MovieApp.Responses;
using MovieApp.Services;

namespace MovieApp.Repositories
{
  [ExcludeFromCodeCoverage]
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

    public async Task DeleteMovieAsync(string imdbId)
    {
      var filter = filterBuilder.Eq(movie => movie.ImdbId, imdbId);
      await itemsCollection.DeleteOneAsync(filter);
    }

    public async Task<Movie> GetMovieFromDbAsync(string imdbId)
    {
      var filter = filterBuilder.Eq(movie => movie.ImdbId, imdbId);
      return await itemsCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesFromDbAsync()
    {
      return await itemsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetWatchedMoviesFromDbAsync()
    {
      var filter = filterBuilder.Eq(movie => movie.Watched, true);
      return await itemsCollection.Find(filter).ToListAsync();
    }
    public async Task<IEnumerable<Movie>> GetUnwatchedMoviesFromDbAsync()
    {
      var filter = filterBuilder.Eq(movie => movie.Watched, false);
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

    public async Task UpdateMovieAsync(Movie movie)
    {
      var filter = filterBuilder.Eq(existingMovie => existingMovie.ImdbId, movie.ImdbId);
      await itemsCollection.ReplaceOneAsync(filter, movie);
    }

    public async Task<DetailedMovie> SearchDetailedMovieFromApiAsync(string imdbId) {
      return await imdbApi.SearchDetailedMovie(imdbId);
    }
  }
}