using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VolleyStats.Models;
using VolleyStats.Services;

namespace VolleyStats.ViewModels;

public partial class MatchesPageViewModel : ObservableObject
{
    private readonly DatabaseService database;
    public ObservableCollection<Match> Matches { get; } = new();
    public IReadOnlyList<string> AvailableStatuses { get; } = ["Tervezett", "Lejátszott", "Elmaradt"];
    [ObservableProperty] 
    private Match? selectedMatch;
    [ObservableProperty] 
    private string homeTeam = "";
    [ObservableProperty] 
    private string awayTeam = "";
    [ObservableProperty] 
    private DateTime matchDate = DateTime.Today;
    [ObservableProperty] 
    private string location = "";
    [ObservableProperty] 
    private string status = "Tervezett";
    [ObservableProperty] 
    private string homeSetsWonText = "0";
    [ObservableProperty] 
    private string awaySetsWonText = "0";
    [ObservableProperty] 
    private string errorMessage = "";
    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsNotEditing))] private bool isEditing;
    public bool IsNotEditing => !IsEditing;
    public MatchesPageViewModel(DatabaseService database) => this.database = database;

    public async Task LoadAsync()
    {
        await database.InitializeAsync();
        Matches.Clear();
        foreach (var match in await database.GetMatchesAsync()) Matches.Add(match);
    }

    [RelayCommand] private void NewMatch() 
    { Cancel(); IsEditing = true; }
    [RelayCommand] private void EditMatch()
    {
        if (SelectedMatch is null) 
        { 
            ErrorMessage = "Először válassz ki egy mérkőzést!"; 
            return; 
        }
        HomeTeam = SelectedMatch.HomeTeam; 
        AwayTeam = SelectedMatch.AwayTeam; 
        MatchDate = SelectedMatch.Date;
        Location = SelectedMatch.Location; 
        Status = SelectedMatch.Status;
        HomeSetsWonText = SelectedMatch.HomeSetsWon.ToString(); 
        AwaySetsWonText = SelectedMatch.AwaySetsWon.ToString();
        IsEditing = true;
    }

    [RelayCommand] private async Task SaveMatchAsync()
    {
        if (string.IsNullOrWhiteSpace(HomeTeam) || string.IsNullOrWhiteSpace(AwayTeam) 
            || string.IsNullOrWhiteSpace(Location)) 
        { 
            ErrorMessage = "Minden mező kitöltése kötelező."; 
            return; 
        }
        if (string.Equals(HomeTeam.Trim(), AwayTeam.Trim(), StringComparison.OrdinalIgnoreCase)) 
        { 
            ErrorMessage = "A két csapat nem lehet ugyanaz."; 
            return; }
        if (!int.TryParse(HomeSetsWonText, out var homeSets) || !int.TryParse(AwaySetsWonText, out var awaySets)
            || homeSets is < 0 or > 3 || awaySets is < 0 or > 3) 
        { 
            ErrorMessage = "A szettarány 0 és 3 közötti egész szám legyen."; return; 
        }
        if (Status == "Lejátszott" && (homeSets == awaySets || Math.Max(homeSets, awaySets) != 3)) 
        { 
            ErrorMessage = "Lejátszott meccsnél az egyik csapatnak pontosan 3 szettet kell nyernie."; 
            return; 
        }
        var match = SelectedMatch ?? new Match();
        match.HomeTeam = HomeTeam.Trim(); match.AwayTeam = AwayTeam.Trim(); 
        match.Date = MatchDate;
        match.Location = Location.Trim(); 
        match.Status = Status;
        match.HomeSetsWon = homeSets; match.AwaySetsWon = awaySets;
        await database.SaveAsync(match); 
        await LoadAsync(); Cancel();
    }

    [RelayCommand] private async Task DeleteMatchAsync()
    {
        if (SelectedMatch is null) 
        { 
            ErrorMessage = "Válassz ki egy törlendő mérkőzést!";
            return; 
        }
        await database.DeleteAsync(SelectedMatch); 
        await LoadAsync(); Cancel();
    }
    [RelayCommand] private void Cancel()
    {
        SelectedMatch = null; 
        HomeTeam = AwayTeam = Location = ErrorMessage = ""; 
        HomeSetsWonText = AwaySetsWonText = "0";
        MatchDate = DateTime.Today; Status = "Tervezett"; 
        IsEditing = false;
    }
}
