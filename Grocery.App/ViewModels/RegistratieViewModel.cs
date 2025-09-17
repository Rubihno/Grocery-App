using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        [RelayCommand]
        private void Annuleren()
        {
            if (Application.Current.MainPage != null)
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        [RelayCommand]
        private void Registratie()
        {
            
        }
    }
}
