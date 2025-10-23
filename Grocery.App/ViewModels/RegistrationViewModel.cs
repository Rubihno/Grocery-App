using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly IValidationService _validatieService;
        private readonly IClientService _clientService;
        public RegistrationViewModel(IValidationService validatieService, IClientService clientService)
        {
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

        private bool ValidationChecks(List<Client> clientList)
        {
            bool veldenLeeg = _validatieService.EmptyFieldValidation(EmailAddress, Name, Password, PasswordConfirmation);

            if (veldenLeeg)
            {
                EmptyFieldMessage = _validatieService.EmptyFieldMessage;
                return false;
            }
            else
            {
                EmptyFieldMessage = _validatieService.EmptyFieldMessage;

                _validatieService.EmailValidation(EmailAddress);
                EmailValidationMessage = _validatieService.EmailFailMessage;

                _validatieService.NameValidation(Name, clientList);
                NameValidationMessage = _validatieService.NameFailMessage;

                _validatieService.PasswordValidation(Password, PasswordConfirmation);
                PasswordValidationMessage = _validatieService.PasswordFailMessage;

                return _validatieService.validationList.All(check => check);
            }
        }

        [RelayCommand]
        private async void Cancel()
        {
            bool cancelRegistration = await Application.Current.MainPage?.DisplayAlert("Registratie annuleren?", string.Empty, "Ja", "Nee");
            if (cancelRegistration)
            {
                await Application.Current.MainPage?.Navigation.PopModalAsync();
            }
        }

        [RelayCommand]
        private async Task Registration()
        {
            List<Client> clientList = _clientService.GetAll();
            int id = clientList.Count + 1;

            if (ValidationChecks(clientList))
            {             
                _clientService.AddNieuwAccountToClientList(id, Name, EmailAddress, Password);
                await Application.Current.MainPage?.Navigation.PopModalAsync();
                await Application.Current.MainPage?.DisplayAlert("Account aangemaakt", "U kunt nu inloggen met uw account", "OK");
            }
        }
    }
}
