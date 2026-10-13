using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyStats.Models;


namespace VolleyStats.ViewModels
{
    public partial class MatchesPageViewModel : ObservableObject
    {
        public ObservableCollection<Match> Matches { get; } = new();

        public List<string> AvailableStatuses { get; } = new()
        {
            "Tervezett",
            "Lejátszott",
            "Elmaradt"
        };

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
        private string errorMessage = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEditing))]
        private bool isEditing;

        public bool IsNotEditing => !IsEditing;

        public MatchesPageViewModel()
        {
            // Ideiglenes mintaadatok
            Matches.Add(new Match
            {
                HomeTeam = "VolleyStars",
                AwayTeam = "Dunaújváros",
                Date = DateTime.Today.AddDays(3),
                Location = "Budapest",
                Status = "Tervezett"
            });

            //Matches.Add(new Match
            //{
            //    HomeTeam = "VolleyStars",
            //    AwayTeam = "Szeged",
            //    Date = DateTime.Today.AddDays(-2),
            //    Location = "Szeged",
            //    Status = "Lejátszott",
            //    HomeScore = 3,
            //    AwayScore = 1
            //});
            Matches.Add(new Match
            {
                HomeTeam = "VolleyStars",
                AwayTeam = "Szeged",
                Date = DateTime.Today.AddDays(-2),
                Location = "Szeged",
                Status = "Lejátszott",

                // JAVÍTÁS: Score helyett SetsWon (Szettarány)
                HomeSetsWon = 3,
                AwaySetsWon = 1,

                // TESZT ADATOK a legördülő kártyához:
                Sets = new List<SetDetail>
        {
            new SetDetail
            {
                SetNumber = 1, HomeScore = 25, AwayScore = 21,
                HomeTimeouts = new List<string>{ "14:15", "20:22" },
                AwayTimeouts = new List<string>{ "10:5" }
            },
            new SetDetail { SetNumber = 2, HomeScore = 25, AwayScore = 18 },
            new SetDetail { SetNumber = 3, HomeScore = 22, AwayScore = 25 },
            new SetDetail { SetNumber = 4, HomeScore = 25, AwayScore = 20 }
        }
            });
        }

        [RelayCommand]
        private void NewMatch()
        {
            Cancel();
            MatchDate = DateTime.Today;
            Status = "Tervezett";
            IsEditing = true;
        }

        [RelayCommand]
        private void EditMatch()
        {
            if (SelectedMatch == null)
            {
                ErrorMessage = "Először válassz ki egy mérkőzést!";
                return;
            }

            HomeTeam = SelectedMatch.HomeTeam;
            AwayTeam = SelectedMatch.AwayTeam;
            MatchDate = SelectedMatch.Date;
            Location = SelectedMatch.Location;
            Status = SelectedMatch.Status;

            ErrorMessage = "";
            IsEditing = true;
        }

        [RelayCommand]
        private void SaveMatch()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(HomeTeam))
            {
                ErrorMessage = "Add meg a hazai csapat nevét!"; return;
            }
            if (string.IsNullOrWhiteSpace(AwayTeam))
            {
                ErrorMessage = "Add meg a vendég csapat nevét!"; return;
            }
            if (HomeTeam.Trim().Equals(AwayTeam.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                ErrorMessage = "A hazai és a vendég csapat nem lehet ugyanaz!"; return;
            }

            if (string.IsNullOrWhiteSpace(Location))
            {
                ErrorMessage = "Add meg a mérkőzés helyszínét!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                ErrorMessage = "Válassz állapotot!";
                return;
            }

            Match newMatch = new Match
            {
                HomeTeam = HomeTeam.Trim(),
                AwayTeam = AwayTeam.Trim(),
                Date = MatchDate,
                Location = Location.Trim(),
                Status = Status
            };

            if (SelectedMatch != null)
            {
                int index = Matches.IndexOf(SelectedMatch);

                if (index < 0)
                {
                    ErrorMessage = "A mérkőzés már nem található a listában.";
                    return;
                }

                Matches[index] = newMatch;
            }
            else
            {
                Matches.Add(newMatch);
            }

            Cancel();
        }

        [RelayCommand]
        private void DeleteMatch()
        {
            if (SelectedMatch == null)
            {
                ErrorMessage = "Válassz ki egy törlendő mérkőzést!";
                return;
            }

            Matches.Remove(SelectedMatch);
            Cancel();
        }

        [RelayCommand]
        private void Cancel()
        {
            SelectedMatch = null;
            HomeTeam = "";
            AwayTeam = "";
            MatchDate = DateTime.Today;
            Location = "";
            Status = "Tervezett";
            ErrorMessage = "";
            IsEditing = false;
        }
    }
}
