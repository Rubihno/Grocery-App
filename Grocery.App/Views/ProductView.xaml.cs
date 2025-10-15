using Grocery.App.ViewModels;
using Grocery.Core.Enums;
using Grocery.Core.Models;

namespace Grocery.App.Views;

public partial class ProductView : ContentPage
{
	private readonly GlobalViewModel _globalViewModel;
	public ProductView(ProductViewModel viewModel, GlobalViewModel globalViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_globalViewModel = globalViewModel;
	}

    private async void OnCreateProductClicked(object? sender, EventArgs e)
    {
		if (_globalViewModel.Client.currentRole == Role.Admin) await Shell.Current.GoToAsync(nameof(NewProductView));
    }
}