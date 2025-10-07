using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        public Category? Get(int id);
        public List<Category> GetAll();
    }
}
