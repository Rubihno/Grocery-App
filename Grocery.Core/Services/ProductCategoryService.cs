using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategories;

        public ProductCategoryService(IProductCategoryRepository productCategories)
        {
            _productCategories = productCategories;
        }

        public ProductCategory? Get(int id)
        {
            return _productCategories.Get(id);
        }
        public List<ProductCategory> GetAll()
        {
            return _productCategories.GetAll();
        }
        public void Add(ProductCategory item)
        {
            _productCategories.Add(item);
        }
        public void Remove(ProductCategory item)
        {
            _productCategories.Remove(item);
        }
        public ProductCategory Update(ProductCategory item)
        {
            return _productCategories.Update(item);
        }
    }
}
