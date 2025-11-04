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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    { // Endpoints
        var group = app.MapGroup("games").WithParameterValidation();
        // Get all games
        group.MapGet("/", () => games);

        // Get game by id
        group
            .MapGet(
                "/{id}",
                (int id) =>
                {
                    GameDto? game = games.Find(game => game.Id == id);

                    return game is null ? Results.NotFound() : Results.Ok(game);
                }
            )
            .WithName(GetGameEndpointName);

        // Post a new game

        group.MapPost(
            "/",
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
        group.MapPut(
            "/{id}",
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

        group.MapDelete(
            "/{id}",
            (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            }
        );

        return group;
    }

    private static void WithParameterValidation()
    {
        throw new NotImplementedException();
    }
}
