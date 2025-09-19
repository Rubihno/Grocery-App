using Grocery.App.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace Grocery.App.Views;

public partial class RegistratieView : ContentPage
{
	public RegistratieView(RegistratieViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}