using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics.Tracing;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _groceryListRepository = groceryListRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            Product product = _productRepository.Get(productId);
            List<GroceryListItem> groceryListItems = _groceryListItemsRepository.GetAllOnGroceryItemId(productId);
            List<BoughtProducts> boughtProductsList = new();

            foreach(GroceryListItem item in groceryListItems)
            {
                GroceryList groceryList = _groceryListRepository.Get(item.GroceryListId);
                Client client = _clientRepository.Get(groceryList.ClientId);
                BoughtProducts boughtProduct = new BoughtProducts(client, groceryList, product);

                boughtProductsList.Add(boughtProduct);
            }

            return boughtProductsList;
        }
    }
}