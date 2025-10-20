using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics;
using Grocery.App.Views;
namespace Grocery.App.ViewModels
{
    public partial class NewGroceryListViewModel : BaseViewModel
    {
        private readonly IGroceryListService _groceryListService;
        private readonly Client _client;

        public NewGroceryListViewModel(IGroceryListService groceryListService, GlobalViewModel global)
        {
            _groceryListService = groceryListService;
            _client = global.Client;
        }

        public string Name { get; set; }
        public Color Color { get; set; }

        [ObservableProperty]
        private string nameErrorMessage;
        [ObservableProperty]
        private string colorCodeErrorMessage;

        [RelayCommand]
        public void CreateGroceryList()
        {
            int groceryListCount = _groceryListService.GetAll().Count();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            Debug.WriteLine($"Het niet id is: {groceryListCount + 1}");

            // TODO: Validatie checks toevoegen om foute/geen data te voorkomen
            GroceryList newGroceryList = new(groceryListCount + 1, Name, currentDate, Color.ToHex(), _client.Id);
            _groceryListService.Add(newGroceryList);

            Shell.Current.GoToAsync(nameof(GroceryListsView));
        }
    }
}
