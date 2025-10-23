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
            return _groceriesRepository.Delete(item);
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public Dictionary<int, int> SortAmountSold(List<GroceryListItem> groceryListItems, Dictionary<int, int> amountSold)
        {
            foreach (GroceryListItem item in groceryListItems)
            {
                if (amountSold.ContainsKey(item.ProductId)) amountSold[item.ProductId] += item.Amount;
                else amountSold.Add(item.ProductId, item.Amount);
            }

            Dictionary<int, int> sortedAmountSold = new Dictionary<int, int>(
                from entry in amountSold orderby entry.Value descending select entry
            );
            return sortedAmountSold;
        }

        public List<Product> GetSortedProductList(Dictionary<int, int> sortedAmountSold, int topX, Dictionary<int, int> amountSold)
        {
            List<Product> productList = new List<Product>();
            int amount = 0;

            foreach (var item in sortedAmountSold)
            {
                if (amount == topX || amount == amountSold.Count()) break;
                Product product = _productRepository.Get(item.Key);

                productList.Add(product);
                amount++;
            }
            return productList;
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();
            Dictionary<int, int> amountSold = new Dictionary<int, int>();

            if (groceryListItems.Count() == 0) return bestSellingProducts;
            Dictionary<int, int> sortedAmountSold = SortAmountSold(groceryListItems, amountSold);
            List<Product> productList = GetSortedProductList(sortedAmountSold, topX, amountSold);

            productList.ForEach(product => bestSellingProducts.Add(
                new BestSellingProducts(product.Id, product.Name, product.Stock, sortedAmountSold[product.Id], productList.IndexOf(product) + 1)));

            return bestSellingProducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0, 0.00m);
            }
        }
    }
}
