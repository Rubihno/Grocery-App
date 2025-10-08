using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Services
{
    public interface IProductCategoryService
    {
        public ProductCategory? Get(int id);
        public List<ProductCategory> GetAll();
        public void Add(ProductCategory item);
        public void Remove(ProductCategory item);
        public ProductCategory Update(ProductCategory item);
    }
}
