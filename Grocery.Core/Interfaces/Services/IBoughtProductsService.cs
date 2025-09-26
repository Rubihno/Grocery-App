using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IBoughtProductsService
    {
        public List<BoughtProducts> Get(int? productId);
    }
}