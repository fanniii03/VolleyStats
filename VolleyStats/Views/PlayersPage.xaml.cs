using VolleyStats.ViewModels;
using Microsoft.Maui.Controls;
namespace VolleyStats.Views
{
	public partial class PlayersPage : ContentPage
	{
		public PlayersPage()
		{
			InitializeComponent();
			BindingContext = new PlayersPageViewModel();

		}
	}
}