using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly List<Category> categories;

        public CategoryRepository()
        {
            categories = [
                    new Category(1, "Groente"),
                    new Category(2, "Bakkerij"),
                    new Category(3, "Zuivel"),
                    new Category(4, "Vleeswaren"),
                    new Category(5, "Fruit")
                ];
        }

        public Category? Get(int id)
        {
            return categories.FirstOrDefault(c => c.Id == id);
        }
        public List<Category> GetAll()
        {
            return categories;
        }
    }
}
