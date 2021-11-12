using System;
using MovieApp.Dtos.Movie;
using MovieApp.Entities;
using MovieApp.Dtos.Movie.Responses;

namespace MovieApp
{
    public static class Extensions
    {
        public static MovieDto AsDto(this Movie movie)
        {
            return new MovieDto(movie.Id, movie.ImdbId, movie.Title, movie.Image, movie.Description, movie.Watched);
        }
        public static MovieSearchDto AsSearchDto(this Movie movie)
        {
            return new MovieSearchDto(movie.ImdbId, movie.Title, movie.Image, movie.Description);
        }
        public static CreateMovieDto AsCreateMovieDto(this Movie movie)
        {
            return new CreateMovieDto(movie.ImdbId, movie.Watched);
        }
        public static Movie AsMovie(this MovieResponse movieResponse)
        {
            return new Movie() { Id = Guid.NewGuid(), ImdbId = movieResponse.id, Title = movieResponse.title, Description = movieResponse.description, Image = movieResponse.image };
        }
    }
}