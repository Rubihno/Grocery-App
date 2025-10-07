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
        public Category? Get(int id);
        public List<Category> GetAll();
        public void Add(Product product);
        public void Remove(Product product);
        public void Update();
    }
}
