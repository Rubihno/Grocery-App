using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Enums;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Microsoft.Maui.Platform;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        public GlobalViewModel _global { get; set; }
        private readonly IGroceryListService _groceryListService;

        public GroceryListViewModel(IGroceryListService groceryListService, GlobalViewModel global) 
        {
            _global = global;
            Title = $"Boodschappen van {_global.Client.Name}";

            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetAll());
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }

        [RelayCommand]
        public async void ShowBoughtProducts()
        {
            if (_global.Client.currentRole == Role.Admin)
            {
                await Shell.Current.GoToAsync($"{nameof(BoughtProductsView)}");
            }
        }

        [RelayCommand]
        public async void ShowNewGroceryListView()
        {
            await Shell.Current.GoToAsync(nameof(NewGroceryListView));
        }
    }
}
