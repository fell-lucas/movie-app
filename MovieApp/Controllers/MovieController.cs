using Microsoft.AspNetCore.Mvc;
using MovieApp.Repositories;
using MovieApp.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MovieApp.Entities;
using System;

namespace MovieApp.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MovieController : ControllerBase
  {
    private readonly IMoviesRepository repository;

    public MovieController(IMoviesRepository repository)
    {
      this.repository = repository;
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
    public async Task<ActionResult<IEnumerable<MovieSearchDto>>> SearchMoviesAsync(string fts)
    {
      var movies = (await repository.SearchMoviesFromApiAsync(fts)).Select(movie => movie.AsSearchDto());
      if (!movies.Any())
      {
        return NotFound();
      }
      return Ok(movies);
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