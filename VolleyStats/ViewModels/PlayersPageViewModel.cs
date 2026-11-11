using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VolleyStats.Models;
using VolleyStats.Services;
using VolleyStats.Views;

namespace VolleyStats.ViewModels;

public partial class PlayersPageViewModel : ObservableObject
{
    private readonly DatabaseService database;
    public ObservableCollection<Team> Teams { get; } = new();
    public ObservableCollection<Player> Players { get; } = new();
    public IReadOnlyList<string> AvailablePositions { get; } = ["Feladó", "Külső ütő", "Átló", "Középblokkoló", "Liberó"];

    [ObservableProperty] 
    private Team? selectedTeam;
    [ObservableProperty] 
    private Player? selectedPlayer;
    [ObservableProperty] 
    private string newTeamName = "";
    [ObservableProperty] 
    private string playerName = "";
    [ObservableProperty] 
    private string jerseyNumberText = "";
    [ObservableProperty] 
    private string position = "";
    [ObservableProperty] 
    private string errorMessage = "";
    [ObservableProperty] 
    private string teamErrorMessage = "";
    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsNotEditing))] private bool isEditing;
    public bool IsNotEditing => !IsEditing;
    private bool isLoading;

    public PlayersPageViewModel(DatabaseService database)
    { this.database = database; }

    //public async Task LoadAsync()
    //{
    //    await database.InitializeAsync();
    //    var selectedTeamId = SelectedTeam?.Id;
    //    Teams.Clear();
    //    foreach (var team in await database.GetTeamsAsync()) 
    //        Teams.Add(team);
    //    SelectedTeam = Teams.FirstOrDefault(t => t.Id == selectedTeamId) 
    //        ?? Teams.FirstOrDefault();
    //    await LoadPlayersAsync();
    //}
    public async Task LoadAsync()
    {
        await database.InitializeAsync();

        var selectedTeamId = SelectedTeam?.Id;

        isLoading = true;

        try
        {
            Teams.Clear();

            foreach (var team in await database.GetTeamsAsync())
                Teams.Add(team);

            SelectedTeam = Teams.FirstOrDefault(t => t.Id == selectedTeamId)
                           ?? Teams.FirstOrDefault();
        }
        finally
        {
            isLoading = false;
        }

        await LoadPlayersAsync();
    }

    partial void OnSelectedTeamChanged(Team? value)
    {
        if (!isLoading)
            _ = LoadPlayersAsync();
    }
    //partial void OnSelectedPlayerChanged(Player? value)
    //{
    //    if (value is not null && IsNotEditing) _ = OpenProfileAsync(value);
    //}
    private async Task LoadPlayersAsync()
    {
        Players.Clear();
        SelectedPlayer = null;
        if (SelectedTeam is null)
        {
            return;
        }

        foreach (var player in await database.GetPlayersAsync(SelectedTeam.Id)) 
            Players.Add(player);
    }

    [RelayCommand] private async Task AddTeamAsync()
    {
        TeamErrorMessage = "";
        var name = NewTeamName.Trim();
        if (string.IsNullOrWhiteSpace(name)) 
        { 
            TeamErrorMessage = "Add meg a csapat nevét!"; 
            return; 
        }
        if (Teams.Any(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase))) 
        { 
            TeamErrorMessage = "Ez a csapat már létezik!"; 
            return; }
        var team = new Team { Name = name };
        await database.SaveAsync(team);
        Teams.Add(team); SelectedTeam = team; 
        NewTeamName = "";
    }

    [RelayCommand] private void NewPlayer() 
    { 
        Cancel(); 
        IsEditing = true; 
    }
    [RelayCommand] private void EditPlayer()
    {
        if (SelectedPlayer is null) 
        { 
            ErrorMessage = "Először válassz ki egy játékost!";
            return; 
        }
        PlayerName = SelectedPlayer.Name; 
        JerseyNumberText = SelectedPlayer.JerseyNumber.ToString(); 
        Position = SelectedPlayer.Position; 

        IsEditing = true;
    }
    [RelayCommand] private async Task SavePlayerAsync()
    {
        if (SelectedTeam is null) 
        { 
            ErrorMessage = "Először válassz ki egy csapatot!"; 
            return; 
        }

        if (string.IsNullOrWhiteSpace(PlayerName) || string.IsNullOrWhiteSpace(Position)) 
        { 
            ErrorMessage = "A név és a poszt megadása kötelező."; 
            return; 
        }

        if (!int.TryParse(JerseyNumberText, out var number) || number is < 0 or > 99) 
        { 
            ErrorMessage = "A mezszám 0 és 99 közötti egész szám legyen!"; 
            return; 
        }

        if (Players.Any(p => p.JerseyNumber == number 
        && p.Id != SelectedPlayer?.Id)) 
        { 
            ErrorMessage = "Ez a mezszám már foglalt ebben a csapatban!"; 
            return; 
        }

        var player = SelectedPlayer ?? new Player 
        { 
            TeamId = SelectedTeam.Id 
        };

        player.Name = PlayerName.Trim(); 
        player.JerseyNumber = number; player.Position = Position;

        await database.SaveAsync(player); 
        await LoadPlayersAsync(); Cancel();
    }
    [RelayCommand] private async Task DeletePlayerAsync()
    {
        if (SelectedPlayer is null) 
        { 
            ErrorMessage = "Válassz ki egy törlendő játékost!"; 
            return; 
        }

        await database.DeleteAsync(SelectedPlayer);
        await LoadPlayersAsync(); 

        Cancel();
    }
    [RelayCommand] 
    private async Task OpenProfileAsync(Player? player)
    {
        if (player is not null)
        {
            await Shell.Current.GoToAsync(
                nameof(PlayerProfilePage),
            new Dictionary<string, object>
            { ["Player"] = player });
        }
    }
    [RelayCommand] private void Cancel()
    {
        SelectedPlayer = null; 
        PlayerName = JerseyNumberText = Position = ErrorMessage = ""; 
        IsEditing = false;
    }
}
