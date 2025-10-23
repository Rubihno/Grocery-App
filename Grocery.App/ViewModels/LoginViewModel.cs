
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;
        private readonly IValidationService _validatieService;
        private readonly IClientService _clientService;

        [ObservableProperty]
        private string email = "user3@mail.com";

        [ObservableProperty]
        private string password = "user3";

        [ObservableProperty]
        private string loginMessage;

        public LoginViewModel(IAuthService authService, GlobalViewModel global, IValidationService validatieService, IClientService clientService)
        { //_authService = App.GetServices<IAuthService>().FirstOrDefault();
            _authService = authService;
            _global = global;
            _validatieService = validatieService;
            _clientService = clientService;
        }

        [RelayCommand]
        private void Login()
        {
            Client? authenticatedClient = _authService.Login(Email, Password);
            if (authenticatedClient != null)
            {
                LoginMessage = $"Welkom {authenticatedClient.Name}!";
                _global.Client = authenticatedClient;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                LoginMessage = "Ongeldige inloggegevens.";
            }
        }

        [RelayCommand]
        private void AccountAanmaken()
        {
            RegistrationViewModel registratieViewModel = new RegistrationViewModel(_validatieService, _clientService);
            Application.Current.MainPage.Navigation.PushModalAsync(new RegistratieView(registratieViewModel));
        }
    }
}
