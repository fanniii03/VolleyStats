using VolleyStats.ViewModels;

namespace VolleyStats.Views;

public partial class MatchesPage : ContentPage
{
    private readonly MatchesPageViewModel viewModel;
    public MatchesPage(MatchesPageViewModel viewModel)
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
