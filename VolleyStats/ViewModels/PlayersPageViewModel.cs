using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VolleyStats.Models;

namespace VolleyStats.ViewModels
{
    public partial class PlayersPageViewModel : ObservableObject
    {
        public ObservableCollection<Team> Teams { get; } = new();
        public ObservableCollection<Player> Players { get; } = new();
        public List<string> AvailablePositions { get; } = new()
        {
            "Feladó (F)",
            "Négyes ütő / Külső ütő (4Ü)",
            "Kettes ütő / Átló (Á / 2Ü)",
            "Center / Középblokkoló (C)",
            "Liberó (L)"
        };
        //[ObservableProperty]
        //private Team? selectedTeam;

        [ObservableProperty]
        private Player? selectedPlayer;

        [ObservableProperty]
        private string playerName = "";

        [ObservableProperty]
        private string jerseyNumberText = "";

        [ObservableProperty]
        private string position = "";

        [ObservableProperty]
        private string errorMessage = "";

        [ObservableProperty]
        private string newTeamName = "";

        [ObservableProperty]
        private string teamErrorMessage = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEditing))]
        private bool isEditing;

        [RelayCommand]
        private void AddTeam()
        {
            TeamErrorMessage = "";

            if (string.IsNullOrWhiteSpace(NewTeamName))
            {
                TeamErrorMessage = "Add meg a csapat nevét!";
                return;
            }

            string teamName = NewTeamName.Trim();

            bool teamExists = Teams.Any(team =>
                string.Equals(
                    team.Name,
                    teamName,
                    StringComparison.OrdinalIgnoreCase));

            if (teamExists)
            {
                TeamErrorMessage = "Ez a csapat már létezik!";
                return;
            }

            Team newTeam = new Team
            {
                Name = teamName
            };

            Teams.Add(newTeam);
            SelectedTeam = newTeam;

            NewTeamName = "";
            TeamErrorMessage = "";
        }

        private Team? selectedTeam;
        public Team? SelectedTeam
        {
            get => selectedTeam;
            set
            {
                if (SetProperty(ref selectedTeam, value))
                {
                    RefreshPlayers();
                    Cancel();
                }
            }
        }

        public bool IsNotEditing => !IsEditing;
        public PlayersPageViewModel()
        {
            // Ideiglenes mintaadatok
            Team team1 = new Team { Name = "VolleyStars" };

            team1.Players.Add(new Player
            {
                Name = "Kovács Anna",
                JerseyNumber = 7,
                Position = "Feladó (F)"
            });

            team1.Players.Add(new Player
            {
                Name = "Nagy Petra",
                JerseyNumber = 12,
                Position = "Négyes ütő / Külső ütő (4Ü)"
            });

            Team team2 = new Team { Name = "NetHunters" };

            team2.Players.Add(new Player
            {
                Name = "Szabó Dóra",
                JerseyNumber = 4,
                Position = "Liberó (L)"
            });

            Teams.Add(team1);
            Teams.Add(team2);

            SelectedTeam = team1;
            RefreshPlayers();
        }

        //partial void OnSelectedTeamChanged(Team? value)
        //{
        //    Cancel();
        //}

        [RelayCommand]
        private void NewPlayer()
        {
            Cancel();
            IsEditing= true;
        }

        [RelayCommand]
        private void EditPlayer()
        {
            if (SelectedPlayer == null)
            {
                ErrorMessage = "Először válassz ki egy játékost!";
                return;
            }

            PlayerName = SelectedPlayer.Name;
            JerseyNumberText = SelectedPlayer.JerseyNumber.ToString();
            Position = SelectedPlayer.Position;

            ErrorMessage = "";
            IsEditing = true;
        }
        [RelayCommand]
        private void Cancel()
        {
            SelectedPlayer = null;
            PlayerName = "";
            JerseyNumberText = "";
            Position = "";
            ErrorMessage = "";

            IsEditing = false;
        }


        [RelayCommand]
        private void SavePlayer()
        {
            ErrorMessage = "";

            if (SelectedTeam == null)
            {
                ErrorMessage = "Először válassz ki egy csapatot!";
                return;
            }

            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                ErrorMessage = "Add meg a játékos nevét!";
                return;
            }

            if (!int.TryParse(JerseyNumberText, out int jerseyNumber)
                || jerseyNumber < 0 || jerseyNumber > 99)
            {
                ErrorMessage = "A mezszám 0 és 99 közötti egész szám legyen!";
                return;
            }

            if (string.IsNullOrWhiteSpace(Position))
            {
                ErrorMessage = "Válassz egy posztot a listából!";
                return;
            }

            bool duplicateNumber = SelectedTeam.Players.Any(player =>
                player.JerseyNumber == jerseyNumber
                && player != SelectedPlayer);

            if (duplicateNumber)
            {
                ErrorMessage = "Ez a mezszám már foglalt ebben a csapatban!";
                return;
            }

            if (SelectedPlayer != null)
            {
                int index = SelectedTeam.Players.IndexOf(SelectedPlayer);

                if (index < 0)
                {
                    ErrorMessage = "A játékos már nem található a csapatban.";
                    return;
                }

                Player updatedPlayer = new Player
                {
                    Name = PlayerName.Trim(),
                    JerseyNumber = jerseyNumber,
                    Position = Position.Trim()
                };

                SelectedTeam.Players[index] = updatedPlayer;
            }
            else
            {
                Player newPlayer = new Player
                {
                    Name = PlayerName.Trim(),
                    JerseyNumber = jerseyNumber,
                    Position = Position.Trim()
                };

                SelectedTeam.Players.Add(newPlayer);
            }
            RefreshPlayers();
            Cancel();
        }

        [RelayCommand]
        private void DeletePlayer()
        {
            if (SelectedTeam == null || SelectedPlayer == null)
            {
                ErrorMessage = "Válassz ki egy törlendő játékost!";
                return;
            }

            SelectedTeam.Players.Remove(SelectedPlayer);
            RefreshPlayers();
            Cancel();
        }
        private void RefreshPlayers()
        {
            Players.Clear();
            if (SelectedTeam != null)
            {
                foreach (var player in SelectedTeam.Players)
                {
                    Players.Add(player);
                }
            }
        }
    }
}
