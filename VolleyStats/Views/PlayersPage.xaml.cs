using VolleyStats.ViewModels;

namespace VolleyStats.Views;

public partial class PlayersPage : ContentPage
{
    private readonly PlayersPageViewModel viewModel;
    public PlayersPage(PlayersPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadAsync();
    }
}
