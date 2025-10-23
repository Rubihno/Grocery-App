using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        private readonly IFileSaverService _fileSaverService;
        private readonly IGroceryListService _groceryListService;
        
        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);
        [ObservableProperty]
        string myMessage;

        public GroceryListItemsViewModel(IGroceryListItemsService groceryListItemsService, IProductService productService, 
                                         IFileSaverService fileSaverService, IGroceryListService groceryListService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            _fileSaverService = fileSaverService;
            _groceryListService = groceryListService;

            Load(groceryList.Id);
        }

        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id)) MyGroceryListItems.Add(item);
            GetAvailableProducts();
        }

        private void GetAvailableProducts()
        {
            AvailableProducts.Clear();
            foreach (Product p in _productService.GetAll())
                if (MyGroceryListItems.FirstOrDefault(g => g.ProductId == p.Id) == null  && p.Stock > 0)
                    AvailableProducts.Add(p);
        }

        partial void OnGroceryListChanged(GroceryList value)
        {
            Load(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }
        [RelayCommand]
        public void AddProduct(Product product)
        {
            if (product == null) return;
            GroceryListItem item = new(0, GroceryList.Id, product.Id, 1);
            _groceryListItemsService.Add(item);
            product.Stock--;
            _productService.Update(product);
            AvailableProducts.Remove(product);
            OnGroceryListChanged(GroceryList);
        }

        [RelayCommand]
        public async Task ShareGroceryList(CancellationToken cancellationToken)
        {
            if (GroceryList == null || MyGroceryListItems == null) return;
            string jsonString = JsonSerializer.Serialize(MyGroceryListItems);
            try
            {
                await _fileSaverService.SaveFileAsync("Boodschappen.json", jsonString, cancellationToken);
                await Toast.Make("Boodschappenlijst is opgeslagen.").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"Opslaan mislukt: {ex.Message}").Show(cancellationToken);
            }
        }

        [RelayCommand]
        public void Search(string text)
        {
            GetAvailableProducts();

            List<Product> productMatchList = AvailableProducts.Where(p => p.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
            bool matchFound = productMatchList.Any();

            if (matchFound)
            {
                AvailableProducts.Clear();
                productMatchList.ForEach(item => AvailableProducts.Add(item));
            }
            else if (text != string.Empty && !matchFound)
            {
                Shell.Current.DisplayAlert("Onbekend product", "Product staat niet in de lijst!", "OK");
            }

        }

        [RelayCommand]
        public void IncreaseAmount(int productId)
        {
            GroceryListItem? item = MyGroceryListItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return;
            if (item.Amount >= item.Product.Stock) return;

            item.Amount++;
            _groceryListItemsService.Update(item);
            item.Product.Stock--;
            _productService.Update(item.Product);

            int index = MyGroceryListItems.IndexOf(item);
            if (index != -1)
            {
                MyGroceryListItems[index] = item;
            }
        }

        [RelayCommand]
        public void DecreaseAmount(int productId)
        {
            GroceryListItem? item = MyGroceryListItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return;
            if (item.Amount <= 0) return;

            item.Amount--;
            _groceryListItemsService.Update(item);
            item.Product.Stock++;
            _productService.Update(item.Product);

            int index = MyGroceryListItems.IndexOf(item);
            MyGroceryListItems[index] = item;
        }

        [RelayCommand]
        public async void DeleteGroceryList()
        {
            if (GroceryList == null) return;
            List<GroceryListItem> groceryListItems = _groceryListItemsService.GetAllOnGroceryListId(GroceryList.Id);

            bool answer = await Shell.Current.DisplayAlert("Weet je het zeker?", $"Weet je zeker dat je {GroceryList.Name} wil verwijderen?", "Ja", "Nee");

            if (answer)
            {
                _groceryListService.Delete(GroceryList);
                groceryListItems.ForEach(item => _groceryListItemsService.Delete(item));

                await Shell.Current.GoToAsync(nameof(GroceryListsView));
                await Shell.Current.DisplayAlert("Succes", "Boodschappenlijst is succesvol verwijderd!", "Ok");
            }
        }
    }
}
