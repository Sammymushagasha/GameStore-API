namespace GameStore.Api.Dtos;


// DTO: It's a contract between the client and server since it represents 
// a shared agreement about how data will be transferred and used
public record class GameSummaryDto(
    // In here you want to put all the properties that we're agreeing to return back to the client 
    // when they request our list of games

    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);