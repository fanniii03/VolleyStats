using SQLite;
using VolleyStats.Models;

namespace VolleyStats.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection database;

    public DatabaseService()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "volleystats.db3");
        database = new SQLiteAsyncConnection(path);
    }

    public async Task InitializeAsync()
    {
        await database.CreateTableAsync<Team>();
        await database.CreateTableAsync<Player>();
        await database.CreateTableAsync<Match>();
    }

    public async Task<List<Team>> GetTeamsAsync() => await database.Table<Team>().OrderBy(t => t.Name).ToListAsync();
    public async Task<List<Player>> GetPlayersAsync(int teamId) => await database.Table<Player>().Where(p => p.TeamId == teamId).OrderBy(p => p.JerseyNumber).ToListAsync();
    public async Task<List<Match>> GetMatchesAsync() => await database.Table<Match>().OrderByDescending(m => m.Date).ToListAsync();
    public async Task SaveAsync<T>(T entity) where T : new()
    {
        var id = typeof(T).GetProperty("Id")?.GetValue(entity) as int? ?? 0;
        if (id == 0) await database.InsertAsync(entity); else await database.UpdateAsync(entity);
    }
    public Task DeleteAsync<T>(T entity) where T : new() => database.DeleteAsync(entity);
}
