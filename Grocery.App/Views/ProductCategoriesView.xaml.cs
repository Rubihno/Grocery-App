using Grocery.App.ViewModels;
namespace Grocery.App.Views;

public partial class ProductCategoriesView : ContentPage
{
    private readonly ProductCategoriesViewModel _viewModel;
	public ProductCategoriesView(ProductCategoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.ResetCategory();
    }
}