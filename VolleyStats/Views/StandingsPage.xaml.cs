using VolleyStats.ViewModels;

namespace VolleyStats.Views;

public partial class StandingsPage : ContentPage
{
    private readonly StandingsPageViewModel viewModel;
    public StandingsPage(StandingsPageViewModel viewModel)
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
