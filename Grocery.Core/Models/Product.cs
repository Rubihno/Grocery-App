using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class Product : Model
    {
        [ObservableProperty]
        private int stock;
        public Product(int id, string name, int stock) : base(id, name)
        {
            Stock = stock;
        }
    }
}
