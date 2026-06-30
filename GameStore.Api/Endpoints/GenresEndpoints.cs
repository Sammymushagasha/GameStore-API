using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEdpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres.Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking().ToListAsync()
        
        );
    }
}