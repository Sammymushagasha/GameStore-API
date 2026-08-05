using System.Net.Http.Json;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class GameStoreApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<GameSummaryDto>> GetGamesAsync(CancellationToken cancellationToken = default)
    {
        var games = await httpClient.GetFromJsonAsync<List<GameSummaryDto>>("games", cancellationToken);
        return games ?? [];
    }

    public async Task<IReadOnlyList<GenreDto>> GetGenresAsync(CancellationToken cancellationToken = default)
    {
        var genres = await httpClient.GetFromJsonAsync<List<GenreDto>>("genres", cancellationToken);
        return genres ?? [];
    }

    public async Task CreateGameAsync(CreateGameRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("games", request, cancellationToken);
        await EnsureSuccessWithMessage(response, cancellationToken);
    }

    public async Task DeleteGameAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"games/{id}", cancellationToken);
        await EnsureSuccessWithMessage(response, cancellationToken);
    }

    private static async Task EnsureSuccessWithMessage(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var apiMessage = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(apiMessage))
        {
            response.EnsureSuccessStatusCode();
        }

        throw new HttpRequestException($"API request failed ({(int)response.StatusCode}): {apiMessage}");
    }
}
