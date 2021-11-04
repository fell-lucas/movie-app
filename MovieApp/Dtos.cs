using System;

namespace MovieApp.Dtos
{
  public record MovieDto(Guid Id, string ImdbId, string Title, string Image, string Description, Boolean Watched);
  public record MovieSearchDto(string ImdbId, string Title, string Image, string Description);
}