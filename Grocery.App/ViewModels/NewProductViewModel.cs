using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IValidationService _validationService;

        public string Name { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public DateTime ShelfLife { get; set; } = DateTime.Now;

        [ObservableProperty]
        private string nameErrorMessage;
        [ObservableProperty]
        private string priceFormatErrorMessage;
        [ObservableProperty]
        private string emptyFieldsErrorMessage;
        [ObservableProperty]
        private string dateErrorMessage;

        public NewProductViewModel(IProductService productService, IValidationService validationService)
        {
            _productService = productService;
            _validationService = validationService;
        }

        // Dit voorkomt toevoegen van foutieve data aan de database
        public bool ValidationChecks()
        {
            bool emptyFieldsResult = _validationService.EmptyFieldValidation(Name, Stock, Price);

            if (emptyFieldsResult)
            {
                EmptyFieldsErrorMessage = _validationService.EmptyFieldMessage;
                return false;
            }
            else
            {
                EmptyFieldsErrorMessage = _validationService.EmptyFieldMessage;

                _validationService.NameValidation(Name, _productService.GetAll());
                NameErrorMessage = _validationService.NameFailMessage;

                _validationService.PriceValidation(Price.Value);
                PriceFormatErrorMessage = _validationService.PriceFailMessage;

                _validationService.DateValidation(ShelfLife);
                DateErrorMessage = _validationService.DateFailMessage;

                bool result = _validationService.validationList.All(check => check);

                return result;
            }
        }

        [RelayCommand]
        async Task CreateProduct()
        {
            if (ValidationChecks())
            {
                _validationService.ClearValidationCheckList();
                Product? product = new(0, Name, Stock.Value, Price.Value, DateOnly.FromDateTime(ShelfLife));
                Product? result = _productService.Add(product);

                if (result == null)
                {
                    await Shell.Current.DisplayAlert("Fout", "Het toevoegen van het product is mislukt", "OK");
                    return;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Succes", "Het product is succesvol toegevoegd!", "OK");
                    await Shell.Current.GoToAsync(nameof(ProductView));
                }
            }
        }

    }
}
