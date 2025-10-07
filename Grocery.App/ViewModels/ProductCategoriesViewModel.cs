using CommunityToolkit.Maui.Core.Extensions;
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
        private readonly IProductService _productService;

        public List<ProductCategory> ProductCategories { get; set; }

        [ObservableProperty]
        Category category = new(0, "None");

        [ObservableProperty]
        private Product? selectedProduct;

        public ObservableCollection<ProductCategory> selectedProductCategories { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService, IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;

            ProductCategories = _productCategoryService.GetAll();
        }

        partial void OnCategoryChanged(Category value) => Load(value.Id);

        private void Load(int id)
        {
            selectedProductCategories.Clear();
            AvailableProducts.Clear();

            AddSelectedProductCategories(id);
            AddAvailableProduct(id);
        }

        partial void OnSelectedProductChanged(Product? value)
        {
            if (value != null)
            {
                AddProduct(value);
            }
            
            SelectedProduct = null;
        }

        private void AddSelectedProductCategories(int id)
        {
            foreach (ProductCategory item in ProductCategories)
            {
                if (item.CategoryId == id)
                {
                    item.Id = selectedProductCategories.Count() + 1;
                    selectedProductCategories.Add(item);
                }
            }
        }

        private void AddAvailableProduct(int id)
        {
            List<Product> products = _productService.GetAll();

            List<Product> selectedProducts = new();

            foreach (ProductCategory item in ProductCategories.Where(x => x.CategoryId == id))
            {
                Product product = _productService.Get(item.ProductId);
                if (!selectedProducts.Contains(product)) selectedProducts.Add(product);
            }

            foreach (Product item in products.Where(x => !selectedProducts.Contains(x)).ToObservableCollection())
            {
                AvailableProducts.Add(item);
            }
        }

        public void AddProduct(Product item)
        {
            int productCategoryId = ProductCategories.Count() + 1;
            ProductCategory productCategory = new(productCategoryId, item.Name, item.Id, Category.Id);

            if (!selectedProductCategories.Contains(productCategory)) _productCategoryService.Add(productCategory);
            
            ProductCategories = _productCategoryService.GetAll();
            Load(Category.Id);
        }

        public void ResetCategory()
        {
            Category = new(0, "None");
        }

        [RelayCommand]
        public void RemoveProduct(ProductCategory item)
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
