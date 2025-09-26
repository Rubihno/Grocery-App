using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> groceryListItems;

        public GroceryListItemsRepository()
        {
            groceryListItems = [
                new GroceryListItem(1, 1, 1, 3),
                new GroceryListItem(2, 1, 2, 1),
                new GroceryListItem(3, 1, 3, 4),
                new GroceryListItem(4, 2, 1, 2),
                new GroceryListItem(5, 2, 2, 5),
            ];
        }

        public List<GroceryListItem> GetAll()
        {
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int id)
        {
            return groceryListItems.Where(g => g.GroceryListId == id).ToList();
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            int newId = groceryListItems.Max(g => g.Id) + 1;
            item.Id = newId;
            groceryListItems.Add(item);
            return Get(item.Id);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return groceryListItems.FirstOrDefault(g => g.Id == id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            GroceryListItem? listItem = groceryListItems.FirstOrDefault(i => i.Id == item.Id);
            listItem = item;
            return listItem;
        }
    }
}
