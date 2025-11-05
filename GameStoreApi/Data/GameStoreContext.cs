using GameStoreApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();
}
