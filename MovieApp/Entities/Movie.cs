using System;

namespace MovieApp.Entities
{
  public record Movie
  {
    public Guid id { get; init; }
    public string ImdbId { get; init; }
    public string Title { get; init; }
    public string Image { get; init; }
    public string Description { get; init; }
    public Boolean Watched { get; init; }
  }
}