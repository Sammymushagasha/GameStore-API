namespace GameStore.Api.Model;

public class Game
{
    public int Id { get; set; }

    public required string Name { set; get; } 

    public Genres? Genre {get; set;}

    public int GenreId {get; set; }

    public decimal Price {get; set; }

    public DateOnly ReleaseDate { get; set; }
}