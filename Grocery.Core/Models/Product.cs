using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class Product : Model
    {
        [ObservableProperty]
        public int stock;
        public DateOnly ShelfLife { get; set; }
        public Product(int id, string name, int stock) : this(id, name, stock, default) { }

        public Product(int id, string name, int stock, DateOnly shelfLife) : base(id, name)
        {
            Stock = stock;
            ShelfLife = shelfLife;
        }
        public override string? ToString()
        {
            return $"{Name} - {Stock} op voorraad";
        }
    }
}
