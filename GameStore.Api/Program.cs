using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Model;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); // Allows for the Required method to work in CreateGameDto

builder.AddGameStoreDb();

var app = builder.Build(); // This is the host of the application

app.MapGamesEndpoints();

app.MapGenresEdpoints();

app.MigrateDb();

app.Run();
