using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VolleyStats.Models;
using VolleyStats.Services;

namespace VolleyStats.ViewModels;

[QueryProperty(nameof(Player), "Player")]
public partial class PlayerProfilePageViewModel : ObservableObject
{
    private readonly DatabaseService database;
    [ObservableProperty] 
    private Player? player;
    [ObservableProperty] 
    private string errorMessage = "";
    public PlayerProfilePageViewModel(DatabaseService database) => this.database = database;

    [RelayCommand] private async Task SaveAsync()
    {
        if (Player is null) return;
        if (string.IsNullOrWhiteSpace(Player.Name) || string.IsNullOrWhiteSpace(Player.Position)) 
        { 
            ErrorMessage = "A név és a poszt kitöltése kötelező."; 
            return; 
        }
        if (Player.HeightCm is < 100 or > 260 || Player.SpikeHeightCm is < 100 or > 400) 
        { 
            ErrorMessage = "Ellenőrizd a magasságokat (cm)."; 
            return; 
        }
        await database.SaveAsync(Player); 
        ErrorMessage = "Adatok elmentve.";
    }

    [RelayCommand] private async Task TakePhotoAsync()
    {
        if (Player is null) return;
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported) 
            { 
                ErrorMessage = "Ezen az eszközön nem érhető el a kamera."; 
                return; }
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;
            var folder = Path.Combine(FileSystem.AppDataDirectory, "player-photos");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, $"player_{Player.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.jpg");
            await using var input = await photo.OpenReadAsync();
            await using var output = File.Create(path);
            await input.CopyToAsync(output);
            Player.PhotoPath = path;
            await database.SaveAsync(Player);
            OnPropertyChanged(nameof(Player));
            ErrorMessage = "Profilkép elmentve.";
        }
        catch (PermissionException) 
        { 
            ErrorMessage = "A kameraengedély szükséges a fénykép készítéséhez."; 
        }
        catch (Exception) 
        { 
            ErrorMessage = "A fénykép készítése nem sikerült."; 
        }
    }

    [RelayCommand] private async Task ShareProfileAsync()
    {
        if (Player is null) return;
        var text = $"{Player.Name} – #{Player.JerseyNumber}\nPoszt: {Player.Position}\nMagasság: {Player.HeightCm} cm\nÜtőmagasság: {Player.SpikeHeightCm} cm";
        await Share.Default.RequestAsync(new ShareTextRequest { Title = "VolleyStats játékosprofil", Text = text });
    }
}
