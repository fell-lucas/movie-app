using Microsoft.AspNetCore.Mvc;
using MovieApp.Repositories;
using MovieApp.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

    [HttpGet]
    public async Task<ActionResult<MovieDto>> GetMovieAsync(string imdbId)
    {
      var movie = await repository.GetMovieAsync(imdbId);

      if (movie is null)
      {
        return NotFound();
      }

      return movie.AsDto();
    }

    [HttpGet("search")]
    public async Task<IEnumerable<MovieSearchDto>> SearchMoviesAsync(string fts)
    {
      var movies = (await repository.SearchMoviesAsync(fts)).Select(movie => movie.AsSearchDto());
      return movies;
    }
  }
}