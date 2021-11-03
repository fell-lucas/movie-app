using MovieApp.Dtos;
using MovieApp.Entities;

namespace MovieApp
{
  public static class Extensions
  {
    public static MovieDto AsDto(this Movie movie)
    {
      return new MovieDto(movie.Id, movie.ImdbId, movie.Title, movie.Image, movie.Description, movie.Watched);
    }
  }
}