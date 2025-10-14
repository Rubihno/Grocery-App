using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using System.Diagnostics;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IValidationService _validationService;

        public string name { get; set; }
        public int stock { get; set; }
        public decimal price { get; set; }
        public DateTime shelfLife { get; set; }

        public NewProductViewModel(IProductService productService, IValidationService validationService)
        {
            _productService = productService;
            _validationService = validationService;
        }

        [RelayCommand]
        private async Task CreateProduct()
        {
            Debug.WriteLine(DateOnly.FromDateTime(shelfLife));
            Product product = new(0, name, stock, price, DateOnly.FromDateTime(shelfLife));
            _productService.Add(product);
            await Shell.Current.GoToAsync(nameof(ProductView));
        }

    }
}
