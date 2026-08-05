using GameStore.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app) // Extends WebApplication class
    {
        // creating a scope which allows to get access to an instance of our GameStore Context

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>(); // this gives us access to an instance of db context
      //  dbContext.Database.EnsureDeleted(); //Deletes the data in the database
        dbContext.Database.Migrate(); // allows for migration of the db when running build

    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder) // These Genres automatically pop up in the db once the build is run 
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        // DbContext has a scoped service lifetime because: 
        // 1. It ensures that a new instance of DbContext is created per request
        // 2. DB connections are a limited and expensive resource
        // 3. DbCOntext is not thread-safe. Scoped avoids to concurrency issues
        // 4. Makes it easier to manage transactions and ensure data consistency
        // 5. Reusing a DbContext instance can lead to increased memory usage

        builder.Services.AddScoped<GameStoreContext>();
        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                string[] requiredGenres = ["Fighting", "Sports", "Action", "RPG", "Puzzle"];

                var genres = context.Set<Genres>().ToList();
                var existingGenreNames = genres
                    .Select(genre => genre.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var missingGenres = requiredGenres
                    .Where(requiredName => !existingGenreNames.Contains(requiredName))
                    .Select(name => new Genres { Name = name })
                    .ToList();

                if (missingGenres.Count > 0)
                {
                    context.Set<Genres>().AddRange(missingGenres);
                    context.SaveChanges();
                }

                var activeGenreIds = context
                    .Set<Game>()
                    .Select(game => game.GenreId)
                    .ToHashSet();

                var removableGenres = context
                    .Set<Genres>()
                    .Where(genre =>
                        !requiredGenres.Contains(genre.Name) &&
                        !activeGenreIds.Contains(genre.Id))
                    .ToList();

                if (removableGenres.Count > 0)
                {
                    context.Set<Genres>().RemoveRange(removableGenres);
                    context.SaveChanges();
                }
            })
            
            );
    }
}