using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class BestSellingProductsView : ContentPage
{
    public BestSellingProductsView(BestSellingProductsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BestSellingProductsViewModel bindingContext)
        {
            bindingContext.OnAppearing();

        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is BestSellingProductsViewModel bindingContext)
        {
            bindingContext.OnDisappearing();
        }
    }
}