using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class RegistratieView : ContentPage
{
	public RegistratieView(RegistratieViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}