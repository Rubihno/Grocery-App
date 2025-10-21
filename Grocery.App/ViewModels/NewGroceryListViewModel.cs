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
        private readonly IValidationService _validationService;
        private readonly Client _client;

        public NewGroceryListViewModel(IGroceryListService groceryListService, IValidationService validationService, GlobalViewModel global)
        {
            _groceryListService = groceryListService;
            _validationService = validationService;
            _client = global.Client;
        }

        public string Name { get; set; }
        public Color Color { get; set; }

        [ObservableProperty]
        private string nameErrorMessage;

        public bool ValidationCheck()
        {
            if (_validationService.EmptyFieldValidation(Name))
            {
                NameErrorMessage = _validationService.EmptyFieldMessage;
                return false;
            }
            else if (!_validationService.NameValidation(Name, _groceryListService.GetAll()))
            {
                NameErrorMessage = _validationService.NameFailMessage;
                return false;
            }
            return true;
        }

        [RelayCommand]
        private async void CreateGroceryList()
        {
            int groceryListCount = _groceryListService.GetAll().Count();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            Debug.WriteLine($"Het niet id is: {groceryListCount + 1}");
            
            if (ValidationCheck())
            {
                _validationService.ClearValidationCheckList();
                NameErrorMessage = string.Empty;
                GroceryList? newGroceryList = new(groceryListCount + 1, Name, currentDate, Color.ToHex(), _client.Id);
                GroceryList? result = _groceryListService.Add(newGroceryList);

                if (result == null)
                {
                    await Shell.Current.DisplayAlert("Fout", "Het toevoegen van de boodschappenlijst is mislukt", "Ok");
                    return;
                }

                await Shell.Current.GoToAsync(nameof(GroceryListsView));
                await Shell.Current.DisplayAlert("Succes", "De boodschappenlijst is succesvol toegevoegd!", "Ok");
            }
        }
    }
}
