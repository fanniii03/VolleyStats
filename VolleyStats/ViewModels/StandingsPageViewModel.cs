using System.Collections.ObjectModel;
using VolleyStats.Models;
using VolleyStats.Services;

namespace VolleyStats.ViewModels;

public class StandingRow
{
    public string TeamName { get; init; } = "";
    public int Played { get; set; }
    public int Won { get; set; }
    public int Lost { get; set; }
    public int SetsWon { get; set; }
    public int SetsLost { get; set; }
    public int Points { get; set; }
    public string SetsDisplay => $"{SetsWon}:{SetsLost}";
}

public class StandingsPageViewModel
{
    private readonly DatabaseService database;
    public ObservableCollection<StandingRow> Standings { get; } = new();
    public StandingsPageViewModel(DatabaseService database) => this.database = database;

    public async Task LoadAsync()
    {
        await database.InitializeAsync();
        var rows = new Dictionary<string, StandingRow>(StringComparer.OrdinalIgnoreCase);
        foreach (var match in (await database.GetMatchesAsync()).Where(m => m.Status == "Lejátszott"))
        {
            var home = GetOrCreate(rows, match.HomeTeam);
            var away = GetOrCreate(rows, match.AwayTeam);
            home.Played++; away.Played++;
            home.SetsWon += match.HomeSetsWon; home.SetsLost += match.AwaySetsWon;
            away.SetsWon += match.AwaySetsWon; away.SetsLost += match.HomeSetsWon;
            if (match.HomeSetsWon > match.AwaySetsWon)
            {
                home.Won++; away.Lost++;
                home.Points += match.AwaySetsWon <= 1 ? 3 : 2;
                away.Points += match.AwaySetsWon == 2 ? 1 : 0;
            }
            else
            {
                away.Won++; home.Lost++;
                away.Points += match.HomeSetsWon <= 1 ? 3 : 2;
                home.Points += match.HomeSetsWon == 2 ? 1 : 0;
            }
        }
        Standings.Clear();
        foreach (var row in rows.Values.OrderByDescending(r => r.Points).ThenByDescending(r => r.Won)
            .ThenByDescending(r => r.SetsWon - r.SetsLost).ThenBy(r => r.TeamName)) Standings.Add(row);
    }

    private static StandingRow GetOrCreate(Dictionary<string, StandingRow> rows, string name)
    {
        if (!rows.TryGetValue(name, out var row)) rows[name] = row = new StandingRow { TeamName = name };
        return row;
    }
}
