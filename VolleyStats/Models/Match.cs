using SQLite;

namespace VolleyStats.Models;

public class Match
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string HomeTeam { get; set; } = "";
    public string AwayTeam { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;
    public string Location { get; set; } = "";
    public string Status { get; set; } = "Tervezett";
    public int HomeSetsWon { get; set; }
    public int AwaySetsWon { get; set; }
}
