using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        public ProductCategory? Get(int id);
        public List<ProductCategory> GetAll();
        public void Add(ProductCategory productCategory);
        public void Remove(ProductCategory productCategory);
        public ProductCategory Update(ProductCategory productCategory);
    }
}
