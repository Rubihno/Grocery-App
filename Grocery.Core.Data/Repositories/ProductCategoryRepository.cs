using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = [
                    new ProductCategory(1, "Brood", 3, 2), 
                    new ProductCategory(2, "Melk", 1, 3)
                ];
        }

        public ProductCategory? Get(int id)
        {
            return productCategories.FirstOrDefault(p => p.Id == id);
        }
        public List<ProductCategory> GetAll()
        {
            return productCategories;
        }
        public void Add(ProductCategory item)
        {
            productCategories.Add(item);
        }
        public void Remove(ProductCategory item)
        {
            productCategories.Remove(item);
        }
        public ProductCategory Update(ProductCategory item)
        {
            ProductCategory? productCategory = productCategories.FirstOrDefault(p => p.Id == item.Id);
            if (productCategory == null) return null;
            productCategory.Id = item.Id;
            return productCategory;
        }
    }
}
