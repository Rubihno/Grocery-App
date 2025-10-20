using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class NewGroceryListView : ContentPage
{
	public NewGroceryListView(NewGroceryListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}