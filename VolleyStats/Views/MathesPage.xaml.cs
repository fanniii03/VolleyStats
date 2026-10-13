using VolleyStats.ViewModels;
namespace VolleyStats.Views;

public partial class MatchesPage : ContentPage
{
    public MatchesPage()
    {
        InitializeComponent();
        //BindingContext = new MatchesPageViewModel();
        BindingContext = new ViewModels.MatchesPageViewModel();
    }
}
