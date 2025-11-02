using System;
using GameStoreApi.Dtos;

namespace GameStoreApi.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games =
    [
        new(1, "Street Fighter II", "Fighting", 19.99m, new DateOnly(1992, 7, 15)),
        new(2, "Finnal Fantasy XIV", "RolePLaying", 59.9M, new DateOnly(2010, 9, 30)),
        new(3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27)),
    ];

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    { // Endpoints
        // Get all games
        app.MapGet("games", () => games);

        // Get game by id
        app.MapGet(
                "games/{id}",
                (int id) =>
                {
                    GameDto? game = games.Find(game => game.Id == id);

                    return game is null ? Results.NotFound() : Results.Ok(game);
                }
            )
            .WithName(GetGameEndpointName);

        // Post a new game

        app.MapPost(
            "games",
            (CreateGameDto newGame) =>
            {
                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            }
        );

        // PUT /Games
        app.MapPut(
            "games/{id}",
            (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                return Results.NoContent();
            }
        );

        // DELETE/games/1

        app.MapDelete(
            "games/{id}",
            (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            }
        );

        return app;
    }
}
