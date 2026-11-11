using VolleyStats.Views;

namespace VolleyStats;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PlayerProfilePage), typeof(PlayerProfilePage));
    }
}
