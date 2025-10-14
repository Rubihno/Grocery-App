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

        [RelayCommand]
        private async Task CreateProduct()
        {
            List<Product> productList = _productService.GetAll();
            List<bool> validationChecks = [];

            bool emptyFieldsResult = _validationService.EmptyFieldValidation(Name, Stock, Price);
            
            if (emptyFieldsResult)
            {
                EmptyFieldsErrorMessage = _validationService.EmptyFieldMessage;
            }
            else
            {
                _validationService.NameValidation(Name, productList);
                NameErrorMessage = _validationService.NameFailMessage;

                _validationService.PriceValidation(Price.Value);
                PriceFormatErrorMessage = _validationService.PriceFailMessage;

                _validationService.DateValidation(ShelfLife);
                DateErrorMessage = _validationService.DateFailMessage;

                if (_validationService.GetValidationCheckList().All(check => check))
                {
                    _validationService.ClearValidationCheckList();
                    Product product = new(0, Name, Stock.Value, Price.Value, DateOnly.FromDateTime(ShelfLife));
                    _productService.Add(product);
                    await Shell.Current.GoToAsync(nameof(ProductView));
                }
            }
        }

    }
}
