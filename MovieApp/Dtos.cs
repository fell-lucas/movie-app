using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Dtos
{
  public record MovieDto(Guid Id, string ImdbId, string Title, string Image, string Description, Boolean Watched);
  public record CreateMovieDto([Required] string ImdbId, Boolean Watched = false);
  public record UpdateMovieDto([Required] Boolean Watched);
  public record MovieSearchDto(string ImdbId, string Title, string Image, string Description);
}