using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(Category), nameof(Category))]
    public partial class ProductCategoriesViewModel : BaseViewModel
    {
        private readonly IProductCategoryService _productCategoryService;

        public List<ProductCategory> ProductCategories { get; set; }
        public ProductCategory nullProductCategory = new ProductCategory(0, "None", 0, 0);

        [ObservableProperty]
        Category category = new(0, "None");

        public ObservableCollection<ProductCategory> selectedProductCategories;
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;

            ProductCategories = _productCategoryService.GetAll();
            selectedProductCategories = new ObservableCollection<ProductCategory>();
            AvailableProducts = new ObservableCollection<Product>();

            Load(category.Id);
        }

        private void Load(int id)
        {
            selectedProductCategories.Clear();
            // Tijdelijke oplossing indien id null is om layout te testen
            if (id == null)
            {
                selectedProductCategories.Add(nullProductCategory);
            }
            else 
            {
                foreach (ProductCategory item in ProductCategories)
                {
                    if (item.CategoryId == id) selectedProductCategories.Add(item);
                }
            }
        }
        partial void OnCategoryChanged(Category value) => Load(value.Id);

        [RelayCommand]
        public void AddProductCommand(ProductCategory item)
        {
            _productCategoryService.Add(item);
            Load(Category.Id);
        }

        [RelayCommand]
        public void RemoveProductCommand(ProductCategory item)
        {
            _productCategoryService.Remove(item);
            Load(Category.Id);
        }

        [RelayCommand]
        public void Search(string text)
        {
            throw new NotImplementedException();
        }
    }
}
