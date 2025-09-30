using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BestSellingProductsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        public ObservableCollection<BestSellingProducts> Products { get; set; } = [];

        public string NoProductsSoldMessage { get;set; } 
        public BestSellingProductsViewModel(IGroceryListItemsService groceryListItemsService)
        {
            _groceryListItemsService = groceryListItemsService;
            Products = [];
            Load();
        }

        public override void Load()
        {
            List<BestSellingProducts> bestSellingProducts = _groceryListItemsService.GetBestSellingProducts();
            Products.Clear();

            if (bestSellingProducts.Count() == 0)
            {
                NoProductsSoldMessage = "Er zijn nog geen producten verkocht";
            }
            else
            {
                NoProductsSoldMessage = string.Empty;
                bestSellingProducts.ForEach(item => Products.Add(item));
            }
        }

        public override void OnAppearing()
        {
            Load();
        }

        public override void OnDisappearing()
        {
            Products.Clear();
        }
    }
}