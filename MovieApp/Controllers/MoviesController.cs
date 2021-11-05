using Microsoft.AspNetCore.Mvc;
using MovieApp.Repositories;
using MovieApp.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MovieApp.Entities;
using MovieApp.Enums;

namespace MovieApp.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MoviesController : ControllerBase
  {
    private readonly IMoviesRepository repository;

    public MoviesController(IMoviesRepository repository)
    {
      this.repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
      var movies = await repository.GetAllMoviesFromDbAsync();
      return movies.Select(movie => movie.AsDto());
    }

    [HttpGet("watched")]
    public async Task<IEnumerable<MovieDto>> GetWatchedMoviesAsync()
    {
      var movies = await repository.GetWatchedMoviesFromDbAsync();
      return movies.Select(movie => movie.AsDto());
    }

    [HttpGet("unwatched")]
    public async Task<IEnumerable<MovieDto>> GetUnwatchedMoviesAsync()
    {
      var movies = await repository.GetUnwatchedMoviesFromDbAsync();
      return movies.Select(movie => movie.AsDto());
    }

    [HttpGet("{imdbId}")]
    public async Task<ActionResult<MovieDto>> GetMovieAsync(string imdbId)
    {
      var movie = await repository.GetMovieFromDbAsync(imdbId);

      if (movie is null)
      {
        return NotFound();
      }

      return movie.AsDto();
    }

    [HttpGet("search")]
    public async Task<IEnumerable<MovieSearchDto>> SearchMoviesAsync(string fts)
    {
      var movies = (await repository.SearchMoviesFromApiAsync(fts)).Select(movie => movie.AsSearchDto());
      return movies;
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> CreateMovieAsync(CreateMovieDto movieDto)
    {
      var existingMovie = await repository.GetMovieFromDbAsync(movieDto.ImdbId);
      if (existingMovie is not null)
      {
        return Conflict();
      }

      var movie = await repository.SearchSingleMovieFromApiAsync(movieDto.ImdbId) with { Watched = movieDto.Watched };

      await repository.CreateMovieAsync(movie);

      return CreatedAtAction(nameof(GetMovieAsync), new { imdbId = movie.ImdbId }, movie.AsDto());
    }
  }
}