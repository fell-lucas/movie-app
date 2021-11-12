using Microsoft.AspNetCore.Mvc;
using MovieApp.Repositories;
using MovieApp.Dtos.Movie;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MovieApp.Dtos.Movie.Responses;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesRepository repository;
        private readonly IImdbSearchService imdbSearchService;

        public MoviesController(IMoviesRepository repository, IImdbSearchService imdbSearchService)
        {
            this.repository = repository;
            this.imdbSearchService = imdbSearchService;
        }

        [HttpGet]
        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            var movies = await repository.GetAllMoviesAsync();
            return movies.Select(movie => movie.AsDto());
        }

        [HttpGet("watched")]
        public async Task<IEnumerable<MovieDto>> GetWatchedMoviesAsync()
        {
            var movies = await repository.GetWatchedMoviesAsync();
            return movies.Select(movie => movie.AsDto());
        }

        [HttpGet("unwatched")]
        public async Task<IEnumerable<MovieDto>> GetUnwatchedMoviesAsync()
        {
            var movies = await repository.GetUnwatchedMoviesAsync();
            return movies.Select(movie => movie.AsDto());
        }

        [HttpGet("{imdbId}")]
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
            var movies = (await imdbSearchService.SearchMoviesFromApiAsync(fts))
              .Select(movie => movie.AsSearchDto());
            return movies;
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovieAsync(CreateMovieDto movieDto)
        {
            var existingMovie = await repository.GetMovieAsync(movieDto.ImdbId);
            if (existingMovie is not null)
            {
                return Conflict();
            }

            var movie = await imdbSearchService.SearchSingleMovieFromApiAsync(movieDto.ImdbId) with { Watched = movieDto.Watched };

            await repository.CreateMovieAsync(movie);

            return CreatedAtAction(nameof(GetMovieAsync), new { imdbId = movie.ImdbId }, movie.AsDto());
        }

        [HttpPut("{imdbId}")]
        public async Task<ActionResult> UpdateMovieAsync(string imdbId, UpdateMovieDto moviedto)
        {
            var existingMovie = await repository.GetMovieAsync(imdbId);
            if (existingMovie is null)
            {
                return NotFound();
            }
            var updatedMovie = existingMovie with { Watched = moviedto.Watched };

            await repository.UpdateMovieAsync(updatedMovie);

            return NoContent();
        }

        [HttpDelete("{imdbId}")]
        public async Task<ActionResult> DeleteMovieAsync(string imdbId)
        {
            var movie = await repository.GetMovieAsync(imdbId);
            if (movie is null)
            {
                return NotFound();
            }

            await repository.DeleteMovieAsync(imdbId);

            return NoContent();
        }

        [HttpGet("detailed/{imdbId}")]
        public async Task<ActionResult<DetailedMovieResponse>> SearchDetailedMovieAsync(string imdbId)
        {
            var movie = await imdbSearchService.SearchDetailedMovieFromApiAsync(imdbId);
            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
    }
}