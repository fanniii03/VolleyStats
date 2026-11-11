using SQLite;

namespace VolleyStats.Models;

public class Team
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
