using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class ProductView : ContentPage
{
	public ProductView(ProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async void OnCreateProductClicked(object? sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(NewProductView));
    }
}