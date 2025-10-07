using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class CategoriesView : ContentPage
{
	public CategoriesView(CategoriesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reset de selectie van de CollectionView
        var collectionView = this.FindByName<CollectionView>("categoriesCollectionView");
        if (collectionView != null)
        {
            collectionView.SelectedItem = null;
        }
    }
}