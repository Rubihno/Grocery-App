namespace Grocery.Core.Models
{
    public class BoughtProducts
    {
        public Product Product { get; set; }
        public Client Client { get; set; }
        public GroceryList GroceryList { get; set; }
        public BoughtProducts(Client client, GroceryList groceryList, Product product)
        {
            Client = client;
            GroceryList = groceryList;
            Product = product;
        }
    }
}