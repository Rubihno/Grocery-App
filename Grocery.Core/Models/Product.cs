using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class Product : Model
    {
        [ObservableProperty]
        public int stock;
        public DateOnly ShelfLife { get; set; }
        public decimal Price { get; set; }
        public Product(int id, string name, int stock, decimal price) : this(id, name, stock, price, default) { }

        public Product(int id, string name, int stock, decimal price, DateOnly shelfLife) : base(id, name)
        {
            Stock = stock;
            ShelfLife = shelfLife;
            if (price < 0) Price = 0;
            else Price = price;
        }
        public override string? ToString()
        {
            return $"{Name} - {Stock} op voorraad met prijs: {Price}";
        }
    }
}
