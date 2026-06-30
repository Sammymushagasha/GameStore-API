using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints{

    const string GetGameEndpointName = "GetGame";


    public static void MapGamesEndpoints(this WebApplication app){
        // GET /games (GET endpoint that is going to be returning games)

        var group = app.MapGroup("/games");

        group.MapGet("/", async (GameStoreContext dbContext) 
        => await dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => new GameSummaryDto(
                                                game.Id,
                                                game.Name,
                                                game.Genre!.Name,
                                                game.Price,
                                                game.ReleaseDate
                            ))
                            .AsNoTracking()
                            .ToListAsync()); 
                            
        // MapGet has 2 parameters, the 1st one matches where you're getting the data from,

        // the 2nd one is the handler which tells ASPNET Core what to do
        // when a request arrives and is matched into the patern given from the 1st parameter
        // ("/games").

        // GET /games/1

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => 
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );

        })
        .WithName(GetGameEndpointName);

        // POST /games

        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate

            };

            dbContext.Games.Add(game); // tells entity framework core a new game is going to be added to the db

            await dbContext.SaveChangesAsync(); // Saves game in db

            GameDetailDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/1

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {

            var existingGame = await dbContext.Games.FindAsync(id);

            if(existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();


            return Results.NoContent();
        });

        // DELETE /games/1

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {

            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}