using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly IClientRepository _clientRepository;
        private readonly IValidationService _validatieService;
        private readonly IClientService _clientService;
        public RegistrationViewModel(IClientRepository clientRepository, IValidationService validatieService, IClientService clientService)
        {
            _clientRepository = clientRepository;
            _validatieService = validatieService;
            _clientService = clientService;
        }

        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        [ObservableProperty]
        public bool isPassword = true;

        [ObservableProperty]
        private string emailValidationMessage;

        [ObservableProperty]
        private string nameValidationMessage;

        [ObservableProperty]
        private string passwordValidationMessage;

        [ObservableProperty]
        private string emptyFieldMessage;

        [RelayCommand]
        private async void Cancel()
        {
            bool annuleren = await Application.Current.MainPage?.DisplayAlert("Registratie annuleren?", string.Empty, "Ja", "Nee");
            if (annuleren)
            {
                Application.Current.MainPage?.Navigation.PopModalAsync();
            }
        }

        [RelayCommand]
        private async Task Registration()
        {
            List<Client> clientList = _clientRepository.GetAll();
            int id = clientList.Count + 1;
            bool[] validatieChecks = new bool[3];

            bool veldenLeeg = _validatieService.EmptyFieldValidation(EmailAddress, Name, Password, PasswordConfirmation);

            if (veldenLeeg)
            {
                EmptyFieldMessage = _validatieService.emptyFieldMessage;
            }
            else
            {
                EmptyFieldMessage = _validatieService.emptyFieldMessage;

                bool emailResult = _validatieService.EmailValidation(EmailAddress);
                EmailValidationMessage = _validatieService.EmailFailMessage;

                bool gebruikersnaamResult = _validatieService.NameValidation(Name, clientList);
                NameValidationMessage = _validatieService.NameFailMessage;

                bool wachtwoordResult = _validatieService.PasswordValidation(Password, PasswordConfirmation);
                PasswordValidationMessage = _validatieService.PasswordFailMessage;

                validatieChecks = [emailResult, gebruikersnaamResult, wachtwoordResult];

                if (validatieChecks.All(check => check))
                {
                    
                    _clientService.AddNieuwAccountToClientList(id, Name, EmailAddress, Password);

                    Application.Current.MainPage?.Navigation.PopModalAsync();

                    Application.Current.MainPage?.DisplayAlert("Account aangemaakt", "U kunt nu inloggen met uw account", "OK");
                }
            }
        }
    }
}
