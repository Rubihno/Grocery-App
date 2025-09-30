using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.Immutable;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            List<Product> productList = new List<Product>();
            List<GroceryListItem> groceryList = _groceriesRepository.GetAll();
            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();
            int aantal = 0;

            Dictionary<int, int> aantalVerkocht = new Dictionary<int, int>();
            if (groceryList.Count() == 0)
            {
                return bestSellingProducts;
            }
            foreach (GroceryListItem item in groceryList)
            {
                if (aantalVerkocht.ContainsKey(item.ProductId)) aantalVerkocht[item.ProductId] += item.Amount;
                else aantalVerkocht.Add(item.ProductId, item.Amount);
            }

            Dictionary<int, int> sortedAantalVerkocht = new Dictionary<int, int> (
                from entry in aantalVerkocht orderby entry.Value descending select entry
            );

            foreach (var item in sortedAantalVerkocht)
            {
                if (aantal == topX || aantal == aantalVerkocht.Count()) break;
                Product product = _productRepository.Get(item.Key);

                productList.Add(product);
                aantal++;
            }

            productList.ForEach(product => bestSellingProducts.Add(
                new BestSellingProducts(product.Id, product.Name, product.Stock, sortedAantalVerkocht[product.Id], productList.IndexOf(product) + 1)));

            return bestSellingProducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
