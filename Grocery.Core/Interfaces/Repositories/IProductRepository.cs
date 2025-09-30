using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();

        public Product? Get(int? id);

        public Product Add(Product item);

        public Product? Delete(Product item);

        public Product? Update(Product item);
    }
}
