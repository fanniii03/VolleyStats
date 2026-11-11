using VolleyStats.ViewModels;

namespace VolleyStats.Views;

public partial class PlayerProfilePage : ContentPage
{
    public PlayerProfilePage(PlayerProfilePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
