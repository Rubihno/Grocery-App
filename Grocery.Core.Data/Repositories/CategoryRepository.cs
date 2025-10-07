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
