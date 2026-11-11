using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace VolleyStats.Models;

public partial class Player : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TeamId { get; set; }

    [ObservableProperty] private string name = "";
    [ObservableProperty] private int jerseyNumber;
    [ObservableProperty] private string position = "";
    [ObservableProperty] private int heightCm;
    [ObservableProperty] private int spikeHeightCm;
    [ObservableProperty] private string photoPath = "";
}
