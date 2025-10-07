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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepostitory;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepostitory = categoryRepository;
        }

        public Category? Get(int id)
        {
            throw new NotImplementedException();
        }
        public List<Category> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
